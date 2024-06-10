using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using CodinaxProjectMvc.Operations;
using CodinaxProjectMvc.DataAccess.Abstract.Storages;
using System.Configuration;
using CodinaxProjectMvc.Constants;
using System.Security.Cryptography.Pkcs;

namespace CodinaxProjectMvc.DataAccess.Storages
{
    public class AzureStorage : Storage, IAzureStorage
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<AzureStorage> _logger;  
        BlobContainerClient _blobContainerClient;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AzureStorage(IConfiguration configuration, ILogger<AzureStorage> logger, IWebHostEnvironment env)
        {
            _blobServiceClient = new(configuration[ConfigurationStrings.AzureAccessKey]);
            _logger = logger;
            _webHostEnvironment = env;
        }

        public async Task DeleteAsync(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            if (blobClient.Exists())
            {
                await blobClient.DeleteAsync();
            }
        }

        public BlobItem GetFile(string pathOrContainerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(pathOrContainerName);
            return _blobContainerClient.GetBlobs().SingleOrDefault(x => x.Name == fileName);
        }

        public List<string> GetFiles(string containerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Select(b => b.Name).ToList();
        }

        public new bool HasFile(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Any(b => b.Name == fileName);
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string containerName, IFormFileCollection files)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await _blobContainerClient.CreateIfNotExistsAsync();
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.Blob);

            List<(string fileName, string pathOrContainerName)> datas = new List<(string fileName, string pathOrContainerName)>();

            foreach (IFormFile file in files)
            {
                string newFileName = await FileRenameAsync(containerName, file.FileName, HasFile);
                BlobClient blobClient = _blobContainerClient.GetBlobClient(newFileName);

                await blobClient.UploadAsync(file.OpenReadStream());
                datas.Add((newFileName, containerName));
            }
            return datas;
        }

        public async Task<(string fileName, string pathOrContainerName)> UploadAsync(string containerName, IFormFile file)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await _blobContainerClient.CreateIfNotExistsAsync();
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.Blob);

            string newFileName = await FileRenameAsync(containerName, file.FileName, HasFile);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(newFileName);

            await blobClient.UploadAsync(file.OpenReadStream());

            return (newFileName, containerName);
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> BitrateAsync(string fileName, string containerName, IFormFile file)
        {
            if (file == null || file.Length <= 0 ||
                !file.ContentType.StartsWith("video/"))
            {
                throw new ArgumentException("Invalid video file.");
            }

            var convertedVideos = new Dictionary<string, IFormFile>();

            string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDirectory);

            try
            {
                string videoFilePath = Path.Combine(tempDirectory, file.FileName);
                using (var stream = new FileStream(videoFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var resolutions = new Dictionary<string, string>
                {
                    { "1080p", "1920x1080" },
                    { "720p", "1280x720" },
                    { "480p", "854x480" },
                    { "360p", "640x360" },
                    { "144p", "256x144" }
                };

                foreach (var resolution in resolutions)
                {
                    string outputFilePath = Path.Combine(tempDirectory, $"{resolution.Key}_{file.FileName}");

                    await ExecuteFFmpegCommand($"-i \"{videoFilePath}\" -vf scale={resolution.Value} -c:a copy \"{outputFilePath}\"");

                    var convertedVideo = new FormFile(
                        baseStream: new FileStream(outputFilePath, FileMode.Open),
                        baseStreamOffset: 0,
                        length: new FileInfo(outputFilePath).Length,
                        name: $"{resolution.Key}_{file.FileName}",
                        fileName: $"{resolution.Key}_{file.FileName}"
                    );

                    convertedVideos.Add(resolution.Key, convertedVideo);
                }
            }
            finally
            {
                Directory.Delete(tempDirectory, true);
            }

            //_blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            //await _blobContainerClient.CreateIfNotExistsAsync();
            //await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.Blob);

            List<(string fileName, string pathOrContainerName)> datas = new List<(string fileName, string pathOrContainerName)>();


            foreach (var convertedVideo in convertedVideos)
            {
                string newFileName = $"{fileName.Replace(".mp4", "")}_{convertedVideo.Key}.mp4";

                _logger.LogInformation("The converted to resolution: {Resulotion}, renamed as: {NewFileName}", convertedVideo.Key, newFileName);

                //BlobClient blobClient = _blobContainerClient.GetBlobClient(newFileName);

                //await blobClient.UploadAsync(file.OpenReadStream());
                //datas.Add((newFileName, containerName));
            }

            return datas;
        }

        private async Task ExecuteFFmpegCommand(string arguments)
        {
            string ffmpegPath = Path.Combine(_webHostEnvironment.WebRootPath, "ffmpeg", "bin", "ffmpeg.exe");

            var processInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = arguments,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo = processInfo;
                process.Start();

                await process.WaitForExitAsync();
            }
        }
    }

    public class Storage
    {
        protected delegate bool HasFile(string pathOrContainerName, string fileName);

        protected async Task<string> FileRenameAsync(string pathOrContainerName, string fileName, HasFile hasFileMethod, bool first = true)
        {
            string newFileName = await Task.Run(async () =>
            {
                string extension = Path.GetExtension(fileName);
                string newFileName = string.Empty;

                if (first)
                {
                    string oldName = Path.GetFileNameWithoutExtension(fileName);
                    newFileName = $"{NameOperations.CharacterRegulatory(oldName)}{extension}";
                }
                else
                {
                    newFileName = fileName;
                    int indexNo1 = newFileName.IndexOf('-');
                    if (indexNo1 == -1)
                        newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                    else
                    {
                        int lastIndex = 0;

                        while (true)
                        {
                            lastIndex = indexNo1;
                            indexNo1 = newFileName.IndexOf("-", indexNo1 + 1);
                            if (indexNo1 == -1)
                            {
                                indexNo1 = lastIndex;
                                break;
                            }
                        }

                        int indexNo2 = newFileName.IndexOf(".");
                        string fileNo = newFileName.Substring(indexNo1 + 1, indexNo2 - indexNo1 - 1);

                        if (int.TryParse(fileNo, out int _fileNo))
                        {
                            _fileNo++;
                            newFileName = newFileName.Remove(indexNo1 + 1, indexNo2 - indexNo1 - 1)
                            .Insert(indexNo1 + 1, _fileNo.ToString());
                        }
                        else
                            newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                    }
                }

                //if(File.Exists($"{pathOrContainerName}\\{newFileName}"))

                if (hasFileMethod(pathOrContainerName, newFileName))
                    return await FileRenameAsync(pathOrContainerName, newFileName, hasFileMethod, false);
                else
                    return newFileName;
            });

            return newFileName;
        }

    }
}
