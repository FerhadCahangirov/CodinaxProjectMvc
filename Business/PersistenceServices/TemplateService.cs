using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Abstract.Storages;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel.CourseVm;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.TemplateVm;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class TemplateService : ITemplateService
    {
        private readonly IReadRepository<Template> _templateReadRepository;
        private readonly IWriteRepository<Template> _templateWriteRepository;
        private readonly IConfiguration _configuration;
        private readonly IAzureStorage _storage;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IWriteRepository<FutureJobTitle> _futureJobTitleWriteRepository;
        private readonly IWriteRepository<Topic> _topicWriteRepository;
        private readonly IWriteRepository<Price> _priceWriteRepository;
        private readonly IReadRepository<Price> _priceReadRepository;
        private readonly IWriteRepository<PriceInfo> _priceInfoWriteRepository;
        private readonly IReadRepository<PriceInfo> _priceInfoReadRepository;
        private readonly IReadRepository<Tool> _toolReadRepository;
        private readonly IWriteRepository<Tool> _toolWriteRepository;

        public TemplateService(
            IReadRepository<Template> templateReadRepository,
            IWriteRepository<Template> templateWriteRepository,
            IActionContextAccessor actionContextAccessor,
            IAzureStorage storage,
            IConfiguration configuration,
            IWriteRepository<FutureJobTitle> futureJobTitleWriteRepository,
            IWriteRepository<Topic> topicWriteRepository,
            IWriteRepository<Price> priceWriteRepository,
            IReadRepository<Price> priceReadRepository,
            IWriteRepository<PriceInfo> priceInfoWriteRepository,
            IReadRepository<PriceInfo> priceInfoReadRepository,
            IReadRepository<Tool> toolReadRepository,
            IWriteRepository<Tool> toolWriteRepository)
        {
            _templateReadRepository = templateReadRepository;
            _templateWriteRepository = templateWriteRepository;
            _storage = storage;
            _configuration = configuration;
            _actionContextAccessor = actionContextAccessor;
            _futureJobTitleWriteRepository = futureJobTitleWriteRepository;
            _topicWriteRepository = topicWriteRepository;
            _priceWriteRepository = priceWriteRepository;
            _priceReadRepository = priceReadRepository;
            _priceInfoWriteRepository = priceInfoWriteRepository;
            _priceInfoReadRepository = priceInfoReadRepository;
            _toolReadRepository = toolReadRepository;
            _toolWriteRepository = toolWriteRepository;
        }


        #region Manage Template Service

        public async Task<bool> CreateTemplateAsync(TemplateCreateVm templateCreateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
                return false;

            (string? fileName, string? pathOrContainerName) uploadedVideo = (null, null);

            if (templateCreateVm.CourseFragmentVideo != null)
            {
                FormFileCollection videos = new FormFileCollection
                {
                    templateCreateVm.CourseFragmentVideo
                };

                List<(string fileName, string pathOrContainerName)> uploadedVideos = await _storage.UploadAsync(AzureContainerNames.CourseFragmentVideos, videos);

                uploadedVideo = uploadedVideos[0];
            }

            (string? fileName, string? pathOrContainerName) uploadedImage = (null, null);

            if (templateCreateVm.CourseImage != null)
            {
                FormFileCollection images = new FormFileCollection
                {
                    templateCreateVm.CourseImage
                };

                List<(string fileName, string pathOrContainerName)> uploadedImages = await _storage.UploadAsync(AzureContainerNames.CourseCoverImages, images);

                uploadedImage = uploadedImages[0];
            }


            Template template = new Template()
            {
                Heading = templateCreateVm.CourseHeading,
                HeadingDescription = templateCreateVm.CourseHeaderDescription,
                Description = templateCreateVm.CourseDescription,
                Location = templateCreateVm.CourseLocation,
                StartingDate = templateCreateVm.CourseStartingDate,
                FinishingDate = templateCreateVm.CourseFinishingDate,
                Content = templateCreateVm.CourseContent,
                FutureJobDesc = templateCreateVm.FutureJobDescription,
                FutureJobSalary = templateCreateVm.FutureJobSalary,
                Properties = templateCreateVm.CourseProperties,
                CourseFragmentVideoName = uploadedVideo.fileName,
                CourseFragmentVideoPathOrContainer = uploadedVideo.pathOrContainerName,
                CourseImageName = uploadedImage.fileName,
                CourseImagePathOrContainer = uploadedImage.pathOrContainerName,
            };

            await _templateWriteRepository.AddAsync(template);
            await _templateWriteRepository.SaveAsync();

            if (templateCreateVm.FutureJobTitles != null && templateCreateVm.FutureJobTitles.Any())
            {
                List<FutureJobTitle> new_futureJobTitles = new List<FutureJobTitle>();

                foreach (var item in templateCreateVm.FutureJobTitles)
                {
                    FutureJobTitle futureJobTitle = new FutureJobTitle()
                    {
                        Content = item.Content,
                        Template = template
                    };

                    new_futureJobTitles.Add(futureJobTitle);
                }

                await _futureJobTitleWriteRepository.AddRangeAsync(new_futureJobTitles);
                await _futureJobTitleWriteRepository.SaveAsync();
            }

            if (templateCreateVm.Topics != null && templateCreateVm.Topics.Any())
            {
                List<Topic> new_topics = new List<Topic>();

                foreach (var item in templateCreateVm.Topics)
                {
                    Topic topic = new Topic()
                    {
                        Content = item.Content,
                        Template = template
                    };

                    new_topics.Add(topic);
                }

                await _topicWriteRepository.AddRangeAsync(new_topics);
                await _topicWriteRepository.SaveAsync();
            }

            return true;
        }

        public async Task<bool> DeleteTemplateAsync(Guid id)
        {
            Template? template = await _templateReadRepository.Table
                .Include(x => x.Courses)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(template == null)
                return false;

            if (template.Courses.Any())
            {
                return false;
            }

            template.IsDeleted = true;
            _templateWriteRepository.Update(template);
            await _templateWriteRepository.SaveAsync();
            return true;
        }

        public Task<TemplatesVm> GetTemplatesAsync()
        {
            IEnumerable<Template> templates = _templateReadRepository
                .GetWhere(x => !x.IsDeleted && !x.IsArchived);

            TemplatesVm templatesVm = new TemplatesVm()
            {
                Templates = templates,
                BaseUrl = _configuration[ConfigurationStrings.AzureBasuUrl]
            };

            return Task.FromResult(templatesVm);
        }

        public async Task<TemplateUpdateVm> GetTemplateUpdateDataAsync(Guid id)
        {
            Template? template = await _templateReadRepository.Table
                .Include(x => x.FutureJobTitles)
                .Include(x => x.Topics)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (template == null)
                return new TemplateUpdateVm();

            TemplateUpdateVm templateUpdateVm = template.FromTemplate_ToTemplateUpdateVm();

            templateUpdateVm.BaseUrl = _configuration[ConfigurationStrings.AzureBasuUrl];

            return templateUpdateVm;
        }

        public async Task<bool> RemoveCourseFragmentVideoAsync(Guid id)
        {
            Template? course = await _templateReadRepository.GetSingleAsync(x => x.Id == id);
            if (course == null)
                return false;

            await _storage.DeleteAsync(course.CourseFragmentVideoPathOrContainer, course.CourseFragmentVideoName);

            course.CourseFragmentVideoName = null;
            course.CourseFragmentVideoPathOrContainer = null;

           _templateWriteRepository.Update(course);
            await _templateWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> UpdateTemplateAsync(TemplateUpdateVm templateUpdateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
                return false;

            Template? template = await _templateReadRepository.Table
                .Include(x => x.FutureJobTitles)
                .Include(x => x.Topics)
                .FirstOrDefaultAsync(x => x.Id == templateUpdateVm.Id);

            if (template == null)
                return false;

            (string? fileName, string? pathOrContainerName) uploadedVideo = (null, null);

            if (templateUpdateVm.CourseFragmentVideo != null)
            {
                if (!string.IsNullOrEmpty(templateUpdateVm.CourseFragmentVideoName) && !string.IsNullOrEmpty(templateUpdateVm.CourseFragmentVideoPathOrContainer))
                {
                    await _storage.DeleteAsync(templateUpdateVm.CourseFragmentVideoPathOrContainer, templateUpdateVm.CourseFragmentVideoName);
                }

                FormFileCollection videos = new FormFileCollection
                {
                    templateUpdateVm.CourseFragmentVideo
                };

                List<(string fileName, string pathOrContainerName)> uploadedVideos = await _storage.UploadAsync(AzureContainerNames.CourseFragmentVideos, videos);

                uploadedVideo = uploadedVideos[0];
            }

            (string? fileName, string? pathOrContainerName) uploadedImage = (null, null);

            if (templateUpdateVm.CourseImage != null)
            {
                if (!string.IsNullOrEmpty(templateUpdateVm.CourseImageName) && !string.IsNullOrEmpty(templateUpdateVm.CourseImagePathOrContainer))
                {
                    await _storage.DeleteAsync(templateUpdateVm.CourseImagePathOrContainer, templateUpdateVm.CourseImageName);
                }

                FormFileCollection images = new FormFileCollection
                {
                    templateUpdateVm.CourseImage
                };

                List<(string fileName, string pathOrContainerName)> uploadedImages = await _storage.UploadAsync(AzureContainerNames.CourseCoverImages, images);

                uploadedImage = uploadedImages[0];
            }

            template.Heading = templateUpdateVm.CourseHeading;
            template.Description = templateUpdateVm.CourseDescription;
            template.HeadingDescription = templateUpdateVm.CourseHeaderDescription;
            template.Location = templateUpdateVm.CourseLocation;
            template.StartingDate = templateUpdateVm.CourseStartingDate;
            template.FinishingDate = templateUpdateVm.CourseFinishingDate;
            template.Content = templateUpdateVm.CourseContent;
            template.FutureJobDesc = templateUpdateVm.FutureJobDescription;
            template.FutureJobSalary = templateUpdateVm.FutureJobSalary;
            template.Properties = templateUpdateVm.CourseProperties;


            if (!string.IsNullOrEmpty(uploadedVideo.fileName) && !string.IsNullOrEmpty(uploadedVideo.pathOrContainerName))
            {
                template.CourseFragmentVideoName = uploadedVideo.fileName;
                template.CourseFragmentVideoPathOrContainer = uploadedVideo.pathOrContainerName;
            }

            if (!string.IsNullOrEmpty(uploadedImage.fileName) && !string.IsNullOrEmpty(uploadedImage.pathOrContainerName))
            {
                template.CourseImageName = uploadedImage.fileName;
                template.CourseImagePathOrContainer = uploadedImage.pathOrContainerName;
            }


            _templateWriteRepository.Update(template);
            await _templateWriteRepository.SaveAsync();

            List<FutureJobTitle> futureJobTitles = await _futureJobTitleWriteRepository.Table
                .Include(x => x.Template)
                .Where(x => x.Template == template).ToListAsync();

            _futureJobTitleWriteRepository.RemoveRange(futureJobTitles);
            await _futureJobTitleWriteRepository.SaveAsync();


            if (templateUpdateVm.FutureJobTitles != null && templateUpdateVm.FutureJobTitles.Any())
            {
                List<FutureJobTitle> new_futureJobTitles = new List<FutureJobTitle>();

                foreach (var item in templateUpdateVm.FutureJobTitles)
                {
                    FutureJobTitle futureJobTitle = new FutureJobTitle()
                    {
                        Content = item.Content,
                        Template = template
                    };

                    new_futureJobTitles.Add(futureJobTitle);
                }

                await _futureJobTitleWriteRepository.AddRangeAsync(new_futureJobTitles);
                await _futureJobTitleWriteRepository.SaveAsync();
            }

            List<Topic> topics = await _topicWriteRepository.Table
                .Include(x => x.Template)
                .Where(x => x.Template == template).ToListAsync();

            _topicWriteRepository.RemoveRange(topics);
            await _topicWriteRepository.SaveAsync();

            if (templateUpdateVm.Topics != null && templateUpdateVm.Topics.Any())
            {
                List<Topic> new_Topics = new List<Topic>();

                foreach (var item in templateUpdateVm.Topics)
                {
                    Topic topic = new Topic()
                    {
                        Content = item.Content,
                        Template = template
                    };

                    new_Topics.Add(topic);
                }

                await _topicWriteRepository.AddRangeAsync(new_Topics);
                await _topicWriteRepository.SaveAsync();
            }

            return true;

        }

        #endregion

        #region Manage Pricing Services

        public async Task<CoursePricesVm> GetPricesAsync(Guid id)
        {
            Template? template = await _templateReadRepository.Table
                .Include(x => x.Prices)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(template == null)
            {
                return new CoursePricesVm();
            }

            var prices = template.Prices.Where(x => !x.IsDeleted);

            PaginationVm<IEnumerable<Price>> data = new PaginationVm<IEnumerable<Price>>(prices);

            CoursePricesVm vm = new CoursePricesVm()
            {
                Data = data,
                TemplateId = id
            };

            return vm;
        }

        public async Task<bool> AddCoursePriceAsync(CoursePriceCreateVm coursePriceCreateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
                return false;

            Template? template = await _templateReadRepository.GetSingleAsync(x => x.Id == coursePriceCreateVm.TemplateId);

            if (template == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(CoursePriceCreateVm.TemplateId), "Course Not Found");
                return false;
            }

            Price price = new Price()
            {
                Template = template,
                Title = coursePriceCreateVm.Title,
            };

            await _priceWriteRepository.AddAsync(price);
            await _priceWriteRepository.SaveAsync();

            List<PriceInfo> priceInfos = new List<PriceInfo>();

            foreach (var item in coursePriceCreateVm.CoursePriceInfos)
            {
                PriceInfo priceInfo = new PriceInfo()
                {
                    Content = item.Content,
                    Price = price
                };

                priceInfos.Add(priceInfo);
            }

            await _priceInfoWriteRepository.AddRangeAsync(priceInfos);
            await _priceInfoWriteRepository.SaveAsync();

            return true;
        }
        public async Task<PaginationVm<IEnumerable<Price>>> GetCoursePricesAsync(Guid id, string? searchFilter = null)
        {
            Template? template = await _templateReadRepository.Table
                .Include(x => x.Prices)
                .ThenInclude(x => x.PriceInfos)
                .Include(x => x.Prices)
                .ThenInclude(x => x.Template)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (template == null)
                return new PaginationVm<IEnumerable<Price>>();

            List<Price> prices = template.Prices.ToList();

            if (!string.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.ToLower();
                prices = prices.Where(price => price.Title.ToLower().Contains(searchFilter)).ToList();
            }

            PaginationVm<IEnumerable<Price>> data = new PaginationVm<IEnumerable<Price>>(prices);

            return data;
        }
        public async Task<CoursePriceUpdateVm> GetCoursePriceUpdateDataAsync(Guid courseId, Guid priceId)
        {
            Price? price = await _priceReadRepository.Table
                .Include(x => x.PriceInfos).FirstOrDefaultAsync(x => x.Id == priceId);

            if (price == null)
                return new CoursePriceUpdateVm();

            CoursePriceUpdateVm coursePriceUpdateVm = new CoursePriceUpdateVm()
            {
                TemplateId = courseId,
                PriceId = priceId,
                Title = price.Title,
                CoursePriceInfos = price.PriceInfos.ToList().Select(x => new CoursePriceInfoVm()
                {
                    Content = x.Content
                })
            };

            return coursePriceUpdateVm;
        }
        public async Task<bool> UpdateCoursePriceAsync(CoursePriceUpdateVm coursePriceUpdateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
                return false;


            Price? price = await _priceReadRepository.Table
                .Include(x => x.PriceInfos).FirstOrDefaultAsync(x => x.Id == coursePriceUpdateVm.PriceId);

            if (price == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(CoursePriceUpdateVm.PriceId), "Course Price Not Found");
                return false;
            }

            price.Title = coursePriceUpdateVm.Title;

            _priceWriteRepository.Update(price);
            await _priceWriteRepository.SaveAsync();

            List<PriceInfo> priceInfos = await _priceInfoReadRepository
                .GetWhere(priceInfo => price.PriceInfos.Contains(priceInfo)).ToListAsync();

            _priceInfoWriteRepository.RemoveRange(priceInfos);

            priceInfos = coursePriceUpdateVm.CoursePriceInfos.Select(priceInfo => new PriceInfo()
            {
                Price = price,
                Content = priceInfo.Content
            }).ToList();

            await _priceInfoWriteRepository.AddRangeAsync(priceInfos);
            await _priceInfoWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> ArchiveCoursePriceAsync(Guid id)
         => await ModifyCoursePriceAsync(id, isArchived: true);

        public async Task<bool> UnArchiveCoursePriceAsync(Guid id)
        => await ModifyCoursePriceAsync(id, isArchived: false);


        public async Task<bool> DeleteCoursePriceAsync(Guid id)
        => await ModifyCoursePriceAsync(id, isDeleted: true);


        public async Task<bool> RestoreCoursePriceAsync(Guid id)
        => await ModifyCoursePriceAsync(id, isDeleted: false);

        private async Task<bool> ModifyCoursePriceAsync(Guid id, bool? isArchived = null, bool? isDeleted = null)
        {
            Price? price = await _priceReadRepository.GetSingleAsync(x => x.Id == id);
            if (price == null)
            {
                return false;
            }

            if (isArchived.HasValue)
            {
                price.IsArchived = isArchived.Value;
            }

            if (isDeleted.HasValue)
            {
                price.IsDeleted = isDeleted.Value;
            }

            _priceWriteRepository.Update(price);
            await _priceWriteRepository.SaveAsync();

            return true;
        }

        #endregion

        #region Manage Tool Services

        public async Task<CourseToolsVm> GetToolsAsync(Guid id)
        {
            Template? template = await _templateReadRepository.Table
                .Include(x => x.Tools)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (template == null)
            {
                return new CourseToolsVm(); 
            }

            CourseToolsVm vm = new CourseToolsVm()
            {
                Tools = template.Tools.Where(x => !x.IsDeleted),
                TemplateId = template.Id,
                BaseUrl = _configuration[ConfigurationStrings.AzureBasuUrl]
            };

            return vm;
        }


        public async Task<bool> ArchiveToolAsync(Guid id)
        {
            Tool? feature = await _toolReadRepository.GetSingleAsync(x => x.Id == id);
            if (feature == null) return false;

            feature.IsArchived = true;

            _toolWriteRepository.Update(feature);
            await _toolWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateToolAsync(CourseToolUpdateVm courseToolUpdateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
                return false;

            Template? template = await _templateReadRepository.GetSingleAsync(x => x.Id == courseToolUpdateVm.TemplateId);

            if (template == null)
            {
                _actionContextAccessor.ActionContext?.ModelState.AddModelError(nameof(CourseToolUpdateVm.TemplateId), "Course Not Found");
                return false;
            }

            Tool? tool = await _toolReadRepository.GetSingleAsync(x => x.Id == courseToolUpdateVm.ToolId);
            if (tool == null)
            {
                _actionContextAccessor.ActionContext?.ModelState.AddModelError(nameof(CourseToolUpdateVm.ToolId), "Tool Not Found");
                return false;
            }

            tool.Content = courseToolUpdateVm.Content;

            if (courseToolUpdateVm.Icon != null)
            {
                await _storage.DeleteAsync(tool.PathOrContainer, tool.FileName);

                IFormFileCollection files = new FormFileCollection{
                    courseToolUpdateVm.Icon
                };

                List<(string fileName, string pathOrContainerName)> uploadedFiles = await _storage.UploadAsync(AzureContainerNames.CourseToolIcons, files);

                (string fileName, string pathOrContainerName) uploadedFile = uploadedFiles[0];

                tool.FileName = uploadedFile.fileName;
                tool.PathOrContainer = uploadedFile.pathOrContainerName;
            }


            _toolWriteRepository.Update(tool);
            await _toolWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> CreateToolAsync(CourseToolCreateVm courseToolCreateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
                return false;

            Template? template = await _templateReadRepository.GetSingleAsync(x => x.Id == courseToolCreateVm.TemplateId);

            if (template == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(CourseToolCreateVm.TemplateId), "Course Not Found");
                return false;
            };

            IFormFileCollection files = new FormFileCollection{
                courseToolCreateVm.Icon
            };

            List<(string fileName, string pathOrContainerName)> uploadedFiles = await _storage.UploadAsync(AzureContainerNames.CourseToolIcons, files);
            (string fileName, string pathOrContainerName) uploadedFile = uploadedFiles[0];

            Tool tool = new Tool()
            {
                PathOrContainer = uploadedFile.pathOrContainerName,
                FileName = uploadedFile.fileName,
                Content = courseToolCreateVm.Content,
                Template = template
            };

            await _toolWriteRepository.AddAsync(tool);
            await _toolWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteToolAsync(Guid id)
        {
            Tool? feature = await _toolReadRepository.GetSingleAsync(x => x.Id == id);
            if (feature == null) return false;

            feature.IsDeleted = true;

            _toolWriteRepository.Update(feature);
            await _toolWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> UnArchiveTool(Guid id)
        {
            Tool? feature = await _toolReadRepository.GetSingleAsync(x => x.Id == id);
            if (feature == null) return false;

            feature.IsArchived = false;

            _toolWriteRepository.Update(feature);
            await _toolWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> RestoreToolAsync(Guid id)
        {
            Tool? feature = await _toolReadRepository.GetSingleAsync(x => x.Id == id);
            if (feature == null) return false;

            feature.IsDeleted = false;

            _toolWriteRepository.Update(feature);
            await _toolWriteRepository.SaveAsync();
            return true;
        }

        public async Task<CourseToolUpdateVm> GetUpdateToolDataAsync(Guid id)
        {
            Tool? tool = await _toolReadRepository.Table
                .Include(x => x.Template)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (tool == null) new CourseUpdateVm();

            CourseToolUpdateVm courseToolUpdateVm = new CourseToolUpdateVm()
            {
                Content = tool.Content,
                IconPathOrContainer = tool.PathOrContainer,
                IconName = tool.FileName,
                ToolId = tool.Id,
                TemplateId = tool.Template.Id
            };

            return courseToolUpdateVm;
        }

        #endregion
    }
}
