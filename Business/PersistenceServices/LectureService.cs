using Azure.Storage.Blobs.Models;
using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Abstract.Storages;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.Enums;
using CodinaxProjectMvc.Helpers;
using CodinaxProjectMvc.ViewModel.LectureVm;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NReco.VideoInfo;

namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class LectureService : ILectureService
    {
        private readonly IReadRepository<Lecture> _lectureReadRepository;
        private readonly IWriteRepository<Lecture> _lectureWriteRepository;
        private readonly IWriteRepository<LectureFile> _lectureFileWriteRepository;
        private readonly IReadRepository<LectureFile> _lectureFileReadRepository;
        private readonly IReadRepository<Module> _moduleReadRepository;
        private readonly IModuleService _moduleService;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IAzureStorage _storage;
        private readonly IConfiguration _configuration;

        public LectureService(
            IReadRepository<Lecture> lectureReadRepository,
            IWriteRepository<Lecture> lectureWriteRepository,
            IModuleService moduleService,
            IAzureStorage storage,
            IConfiguration configuration,
            IActionContextAccessor actionContextAccessor,
            IWriteRepository<LectureFile> lectureFileWriteRepository,
            IReadRepository<LectureFile> lectureFileReadRepository,
            IReadRepository<Module> moduleReadRepository)
        {
            _lectureReadRepository = lectureReadRepository;
            _lectureWriteRepository = lectureWriteRepository;
            _moduleService = moduleService;
            _storage = storage;
            _configuration = configuration;
            _actionContextAccessor = actionContextAccessor;
            _lectureFileWriteRepository = lectureFileWriteRepository;
            _lectureFileReadRepository = lectureFileReadRepository;
            _moduleReadRepository = moduleReadRepository;
        }



        public async Task<bool> CreateLectureAsync(LectureCreateVm lectureCreateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
            {
                return false;
            }

            Module? module = await _moduleService.GetModuleByIdAsync(lectureCreateVm.ModuleId);

            if (module == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(LectureCreateVm.ModuleId), "Module Not Found.");
                return false;
            }

            Lecture lecture = new Lecture()
            {
                Module = module,
                Title = lectureCreateVm.Title,
            };

            await _lectureWriteRepository.AddAsync(lecture);
            await _lectureWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> AddLectureFileAsync(LectureFileCreateVm lectureFileCreateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
            {
                return false;
            }

            switch (lectureFileCreateVm.FileType)
            {
                case FileType.MP4:
                    return await UploadVideoAsync(lectureFileCreateVm.LectureId, lectureFileCreateVm.File, lectureFileCreateVm.Title);
                case FileType.TXT:
                    return await UploadFileAsync(lectureFileCreateVm.LectureId, lectureFileCreateVm.File, lectureFileCreateVm.Title, FileType.TXT);
                case FileType.PDF:
                    return await UploadFileAsync(lectureFileCreateVm.LectureId, lectureFileCreateVm.File, lectureFileCreateVm.Title, FileType.PDF);
                case FileType.URL:
                    return await UploadUrlAsync(lectureFileCreateVm.LectureId, lectureFileCreateVm.Url, lectureFileCreateVm.Title);
                default:
                    return false;
            }
        }

        private async Task<bool> UploadVideoAsync(Guid lectureId, IFormFile? file, string title)
        {
            Lecture? lecture = await _lectureReadRepository.GetSingleAsync(x => x.Id == lectureId);
            if (lecture == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(LectureFileCreateVm.LectureId), "Lecture Not Found.");
                return false;
            }

            if (file == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(LectureFileCreateVm.File), "Lecture Video Is Required.");
                return false;
            }

            (string fileName, string pathOrContainerName) = await _storage.UploadAsync(AzureContainerNames.LectureFiles, file);

            await _storage.BitrateAsync(fileName, pathOrContainerName);

            string duration = GetVideoDuration(pathOrContainerName, fileName);

            LectureFile lectureFile = new LectureFile()
            {
                Duration = duration,
                PathOrContainer = pathOrContainerName,
                FileName = fileName,
                Lecture = lecture,
                Title = title,
                FileType = FileType.MP4
            };

            await _lectureFileWriteRepository.AddAsync(lectureFile);
            await _lectureFileWriteRepository.SaveAsync();

            return true;
        }

        private async Task<bool> UploadFileAsync(Guid lectureId, IFormFile? file, string title, FileType fileType)
        {
            Lecture? lecture = await _lectureReadRepository.GetSingleAsync(x => x.Id == lectureId);
            if (lecture == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(LectureFileCreateVm.LectureId), "Lecture Not Found.");
                return false;
            }

            if (file == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(LectureFileCreateVm.File), "Lecture Video Is Required.");
                return false;
            }

            IFormFileCollection files = new FormFileCollection() { file };

            List<(string fileName, string pathOrContainerName)> uploadedFiles = await _storage.UploadAsync(AzureContainerNames.LectureFiles, files);

            (string fileName, string pathOrContainerName) = uploadedFiles[0];

            string fileSize = GetFileSize(fileName);

            LectureFile lectureFile = new LectureFile()
            {
                FileSize = fileSize,
                PathOrContainer = pathOrContainerName,
                FileName = fileName,
                Lecture = lecture,
                Title = title,
                FileType = fileType
            };

            await _lectureFileWriteRepository.AddAsync(lectureFile);
            await _lectureFileWriteRepository.SaveAsync();

            return true;
        }

        private async Task<bool> UploadUrlAsync(Guid lectureId, string? url, string title)
        {
            Lecture? lecture = await _lectureReadRepository.GetSingleAsync(x => x.Id == lectureId);
            if (lecture == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(LectureFileCreateVm.LectureId), "Lecture Not Found.");
                return false;
            }

            if (url == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(LectureFileCreateVm.Url), "Url Is Required.");
                return false;
            }

            LectureFile lectureFile = new LectureFile()
            {
                FileType = FileType.URL,
                Title = title,
                Url = url,
                Lecture = lecture
            };

            await _lectureFileWriteRepository.AddAsync(lectureFile);
            await _lectureFileWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> UpdateLecturesAsync(LectureUpdateVm lectureUpdateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
            {
                return false;
            }

            Lecture? lecture = await _lectureReadRepository.GetSingleAsync(x => x.Id == lectureUpdateVm.LectureId);

            if(lecture == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(LectureUpdateVm.LectureId), "Lecture Not Found.");
                return false;
            }

            lecture.Title = lectureUpdateVm.Title;

            _lectureWriteRepository.Update(lecture);
            await _lectureWriteRepository.SaveAsync();
            
            return true;
        }

        public async Task<LectureFileUpdateVm> GetLectureFileUpdateDataAsync(Guid lectureFileId)
        {
            LectureFile? lectureFile = await _lectureFileReadRepository.GetSingleAsync(x => x.Id == lectureFileId);
            if (lectureFile == null) return new LectureFileUpdateVm();

            LectureFileUpdateVm lectureFileUpdateVm = new LectureFileUpdateVm()
            {
                LectureFileId = lectureFileId,
                Title = lectureFile.Title,
                FileType = lectureFile.FileType,
                BaseUrl = _configuration[ConfigurationStrings.AzureBaseUrl]
            };

            if(lectureFile.FileType == FileType.URL)
            {
                lectureFileUpdateVm.Url = lectureFile.Url;
            }
            else
            {
                lectureFileUpdateVm.FileName = lectureFile.FileName;
                lectureFileUpdateVm.FilePathOrContainer = lectureFile.PathOrContainer;
            }


            return lectureFileUpdateVm;
        }


        public async Task<bool> UpdateLectureFileAsync(LectureFileUpdateVm lectureFileUpdateVm)
        {
            if(!_actionContextAccessor.ActionContext.ModelState.IsValid)
            {
                return false;
            }

            LectureFile? lectureFile = await _lectureFileReadRepository.GetSingleAsync(x => x.Id == lectureFileUpdateVm.LectureFileId);
            
            if (lectureFile == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(LectureFileUpdateVm.LectureFileId), "Lecture File Not Found.");
                return false;
            }

            lectureFile.Title = lectureFileUpdateVm.Title;

            switch (lectureFile.FileType)
            {
                case FileType.URL:

                    if(lectureFileUpdateVm.Url == null)
                    {
                        _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(LectureFileUpdateVm.Url), "Url is required.");
                        return false;
                    }

                    lectureFile.Url = lectureFileUpdateVm.Url;
                    break;
                case FileType.PDF:
                    
                    if(lectureFileUpdateVm.File != null)
                    {
                        await _storage.DeleteAsync(lectureFileUpdateVm.FilePathOrContainer, lectureFileUpdateVm.FileName);

                        (string fileName, string pathOrContainerName) = await _storage.UploadAsync(AzureContainerNames.LectureFiles, lectureFileUpdateVm.File);

                        string fileSize = GetFileSize(fileName);

                        lectureFile.FileSize = fileSize;
                        lectureFile.FileName = fileName;
                        lectureFile.PathOrContainer = pathOrContainerName;
                    }

                    break;
                case FileType.TXT:

                    if (lectureFileUpdateVm.File != null)
                    {
                        await _storage.DeleteAsync(lectureFileUpdateVm.FilePathOrContainer, lectureFileUpdateVm.FileName);

                        (string fileName, string pathOrContainerName) = await _storage.UploadAsync(AzureContainerNames.LectureFiles, lectureFileUpdateVm.File);

                        string fileSize = GetFileSize(fileName);

                        lectureFile.FileSize = fileSize;
                        lectureFile.FileName = fileName;
                        lectureFile.PathOrContainer = pathOrContainerName;
                    }

                    break;
                case FileType.MP4:
                    
                    if(lectureFileUpdateVm.File != null)
                    {
                        await _storage.DeleteAsync(lectureFileUpdateVm.FilePathOrContainer, lectureFileUpdateVm.FileName);


                        (string fileName, string pathOrContainerName) = await _storage.UploadAsync(AzureContainerNames.LectureFiles, lectureFileUpdateVm.File);

                        string duration = GetVideoDuration(pathOrContainerName, fileName);

                        await _storage.BitrateAsync(fileName, pathOrContainerName);

                        lectureFile.Duration = duration;
                        lectureFile.FileName = fileName;
                        lectureFile.PathOrContainer = pathOrContainerName;
                    }

                    break;
                default:
                    _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(LectureFileUpdateVm.FileType), "No Such File Type Found.");
                    return false;
            }

            _lectureFileWriteRepository.Update(lectureFile);
            await _lectureFileWriteRepository.SaveAsync();

            return true;
        }


        private string GetFileSize(string fileName)
        {
            BlobItem blobItem = _storage.GetFile(AzureContainerNames.LectureFiles, fileName);

            long? sizeinB = blobItem.Properties.ContentLength;

            string fileSize = $"{sizeinB}B";

            if (sizeinB > 1024)
            {
                long? sizeinKB = sizeinB / 1024;
                fileSize = $"{sizeinKB}KB";

                if (sizeinKB > 1024)
                {
                    long? sizeinMB = sizeinB / (1024 * 1024);
                    fileSize = $"{sizeinMB}MB";
                }
            }

            return fileSize;
        }

        private string GetVideoDuration(string pathOrContainerName, string fileName)
        {
            string filePath = $"{_configuration[ConfigurationStrings.AzureBaseUrl]}/{pathOrContainerName}/{fileName}";

            FFProbe ffProbe = new FFProbe();
            MediaInfo videoInfo = ffProbe.GetMediaInfo(filePath);

            return videoInfo.Duration.FormatTimeSpan();
        }

        public async Task<LectureUpdateVm> GetLectureUpdateDataAsync(Guid id)
        {
            Lecture? lecture = await _lectureReadRepository.GetSingleAsync(x => x.Id == id);

            if (lecture == null)
                return new LectureUpdateVm();

            return new LectureUpdateVm(lecture.Id, lecture.Title);
        }

        public async Task<bool> DeleteLectureAsync(Guid id)
        {
            Lecture? lecture = await _lectureReadRepository.GetSingleAsync(x => x.Id == id);

            if (lecture == null)
                return false;

            lecture.IsDeleted = true;

            _lectureWriteRepository.Update(lecture);
            await _lectureWriteRepository.SaveAsync();
            
            return true;
        }

        public async Task<bool> DeleteLectureFileAsync(Guid id)
        {
            LectureFile? lectureFile = await _lectureFileReadRepository.GetSingleAsync(x => x.Id == id);

            if (lectureFile == null)
                return false;

            lectureFile.IsDeleted = true;

            _lectureFileWriteRepository.Update(lectureFile);
            await _lectureFileWriteRepository.SaveAsync();

            return true;
        }
    }
}
