using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using CodinaxProjectMvc.Operations;
using CodinaxProjectMvc.DataAccess.Abstract.Storages;
using CodinaxProjectMvc.Constants;

using Azure.Storage;
using MediaToolkit.Model;
using MediaToolkit.Options;
using MediaToolkit;

namespace CodinaxProjectMvc.DataAccess.Storages
{
    public class AzureStorage : Storage, IAzureStorage
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<AzureStorage> _logger;
        BlobContainerClient _blobContainerClient;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public AzureStorage(IConfiguration configuration, ILogger<AzureStorage> logger, IWebHostEnvironment env)
        {
            _blobServiceClient = new(configuration[ConfigurationStrings.AzureAccessKey]);
            _logger = logger;
            _webHostEnvironment = env;
            _configuration = configuration;

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

            var progressHandler = new Progress<long>(progress =>
            {
                _logger.LogInformation("Uploaded {Progress} bytes of {Length} bytes.", progress, file.Length);
            });

            var uploadOptions = new BlobUploadOptions
            {
                TransferOptions = new StorageTransferOptions
                {
                    MaximumConcurrency = 1024 * 1024 * 1024,

                    InitialTransferSize = 1024 * 1024 * 1024,

                    MaximumTransferSize = 1024 * 1024 * 1024,
                },
                ProgressHandler = progressHandler
            };

            _logger.LogInformation("Upload Process Started...");


            await blobClient.UploadAsync(file.OpenReadStream(), uploadOptions);

            _logger.LogInformation("File {newFileName} uploaded successfully to container {containerName}.", newFileName, containerName);

            return (newFileName, containerName);
        }

        private async Task<MediaFile> GetBlobAsMediaFileAsync(string containerName, string blobName, string downloadPath)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blobClient = _blobContainerClient.GetBlobClient(blobName);

            await blobClient.DownloadToAsync(downloadPath);

            return new MediaFile
            {
                Filename = downloadPath
            };
        }

        public async Task BitrateAsync(string fileName, string containerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await _blobContainerClient.CreateIfNotExistsAsync();
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.Blob);

            _logger.LogInformation("Bitrate Process Started...");

            string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDirectory);

            try
            {
                //MediaFile inputFile = new()
                //{
                //    Filename = $"{_configuration[ConfigurationStrings.AzureBaseUrl]}/{containerName}/{fileName}"
                //};

                MediaFile inputFile = await GetBlobAsMediaFileAsync(containerName, fileName, Path.Combine(tempDirectory, fileName));

                var resulutions = new List<VideoSize> {
                    VideoSize.Hd1080,
                    VideoSize.Hd720,
                    VideoSize.Nhd,
                    VideoSize.Qcif,
                };

                foreach (var res in resulutions)
                {
                    string newFileName = fileName.Replace(".mp4", "") + "_" + res.ToString() + ".mp4";

                    MediaFile outputFile = new MediaFile { Filename = Path.Combine(tempDirectory, newFileName) };

                    var conversionOptions = new ConversionOptions
                    {
                        VideoSize = res,
                    };

                    using (var engine = new Engine())
                    {
                        engine.Convert(inputFile, outputFile, conversionOptions);
                    }

                    IFormFile newFile = await LoadFileAsIFormFile(Path.Combine(tempDirectory, newFileName));

                    _logger.LogInformation("Upload Process Started...");

                    BlobClient blobClient = _blobContainerClient.GetBlobClient(newFileName);

                    var progressHandler = new Progress<long>(progress =>
                    {
                        _logger.LogInformation("Uploaded {Progress} bytes of {Length} bytes.", progress, newFile.Length);
                    });

                    var uploadOptions = new BlobUploadOptions
                    {
                        TransferOptions = new StorageTransferOptions
                        {
                            MaximumConcurrency = 1024 * 1024 * 1024,

                            InitialTransferSize = 1024 * 1024 * 1024,

                            MaximumTransferSize = 1024 * 1024 * 1024,
                        },
                        ProgressHandler = progressHandler
                    };

                    await blobClient.UploadAsync(newFile.OpenReadStream(), uploadOptions);

                    _logger.LogInformation("File {newFileName} uploaded successfully to container {containerName}.", newFileName, containerName);
                }
            }
            finally
            {
                Directory.Delete(tempDirectory, true);
            }
        }

        private async Task<IFormFile> LoadFileAsIFormFile(string filePath)
        {
            var memoryStream = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memoryStream);
            }

            memoryStream.Position = 0;
            IFormFile formFile = new FormFile(memoryStream, 0, memoryStream.Length, "file", Path.GetFileName(filePath));
            return formFile;
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
