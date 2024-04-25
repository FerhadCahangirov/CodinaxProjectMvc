using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Abstract.Storages;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using CodinaxProjectMvc.Enums;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.CourseVm;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class CourseService : ICourseService
    {
        #region Repositories Initialize
        private readonly IReadRepository<Course> _courseReadRepository;
        private readonly IReadRepository<Category> _categoryReadRepository;
        private readonly IWriteRepository<Course> _courseWriteRepository;
        private readonly IReadRepository<Instructor> _instructorReadRepository;
        private readonly IWriteRepository<FutureJobTitle> _futureJobTitleWriteRepository;
        private readonly IWriteRepository<Topic> _topicWriteRepository;
        private readonly IWriteRepository<Tool> _toolWriteRepository;
        private readonly IReadRepository<Tool> _toolReadRepository;
        private readonly IReadRepository<Price> _priceReadRepository;
        private readonly IWriteRepository<Price> _priceWriteRepository;
        private readonly IReadRepository<PriceInfo> _priceInfoReadRepository;
        private readonly IWriteRepository<PriceInfo> _priceInfoWriteRepository;
        #endregion

        private readonly IAzureStorage _storage;
        private readonly IConfiguration _configuration;
        private readonly IActionContextAccessor _actionContextAccessor;

        #region Constructor for Dependency Injection
        public CourseService(IReadRepository<Course> courseReadRepository,
                             IWriteRepository<Course> courseWriteRepository,
                             IActionContextAccessor actionContextAccessor,
                             IReadRepository<Category> categoryReadRepository,
                             IAzureStorage storage,
                             IWriteRepository<FutureJobTitle> futureJobTitleWriteRepository,
                             IWriteRepository<Topic> topicWriteRepository,
                             IConfiguration configuration,
                             IReadRepository<Instructor> instructorReadRepository,
                             IWriteRepository<Tool> toolWriteRepository,
                             IReadRepository<Tool> toolReadRepository,
                             IReadRepository<Price> priceReadRepository,
                             IWriteRepository<Price> priceWriteRepository,
                             IReadRepository<PriceInfo> priceInfoReadRepository,
                             IWriteRepository<PriceInfo> priceInfoWriteRepository)
        {
            _courseReadRepository = courseReadRepository;
            _courseWriteRepository = courseWriteRepository;
            _actionContextAccessor = actionContextAccessor;
            _categoryReadRepository = categoryReadRepository;
            _storage = storage;
            _futureJobTitleWriteRepository = futureJobTitleWriteRepository;
            _topicWriteRepository = topicWriteRepository;
            _configuration = configuration;
            _instructorReadRepository = instructorReadRepository;
            _toolWriteRepository = toolWriteRepository;
            _toolReadRepository = toolReadRepository;
            _priceReadRepository = priceReadRepository;
            _priceWriteRepository = priceWriteRepository;
            _priceInfoReadRepository = priceInfoReadRepository;
            _priceInfoWriteRepository = priceInfoWriteRepository;
        }

        #endregion

        #region Manage Tool Services
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

            Course? course = await _courseReadRepository.GetSingleAsync(x => x.Id == courseToolUpdateVm.CourseId);

            if (course == null)
            {
                _actionContextAccessor.ActionContext?.ModelState.AddModelError(nameof(CourseToolUpdateVm.CourseId), "Course Not Found");
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

            Course? course = await _courseReadRepository.GetSingleAsync(x => x.Id == courseToolCreateVm.CourseId);

            if (course == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(CourseToolCreateVm.CourseId), "Course Not Found");
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
                Course = course
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
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (tool == null) new CourseUpdateVm();

            CourseToolUpdateVm courseToolUpdateVm = new CourseToolUpdateVm()
            {
                Content = tool.Content,
                IconPathOrContainer = tool.PathOrContainer,
                IconName = tool.FileName,
                ToolId = tool.Id,
                CourseId = tool.Course.Id
            };

            return courseToolUpdateVm;
        }

        #endregion

        #region Manage Course Services

        public async Task<bool> CreateCourseAsync(CourseCreateVm courseCreateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
                return false;

            Category? category = await _categoryReadRepository.GetByIdAsync(courseCreateVm.CourseCategory);

            if (category == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(CourseCreateVm.CourseCategory), "This Category Is Not Found");
                return false;
            }

            (string? fileName, string? pathOrContainerName) uploadedVideo = (null, null);

            if (courseCreateVm.CourseFragmentVideo != null)
            {
                FormFileCollection videos = new FormFileCollection
                {
                    courseCreateVm.CourseFragmentVideo
                };

                List<(string fileName, string pathOrContainerName)> uploadedVideos = await _storage.UploadAsync(AzureContainerNames.CourseFragmentVideos, videos);

                uploadedVideo = uploadedVideos[0];
            }

            (string? fileName, string? pathOrContainerName) uploadedImage = (null, null);

            if (courseCreateVm.CourseImage != null)
            {
                FormFileCollection images = new FormFileCollection
                {
                    courseCreateVm.CourseImage
                };

                List<(string fileName, string pathOrContainerName)> uploadedImages = await _storage.UploadAsync(AzureContainerNames.CourseCoverImages, images);

                uploadedImage = uploadedImages[0];
            }


            Course course = new Course()
            {
                CourseCode = courseCreateVm.CourseCode,
                Title = courseCreateVm.CourseTitle,
                Heading = courseCreateVm.CourseHeading,
                HeadingDescription = courseCreateVm.CourseHeaderDescription,
                Description = courseCreateVm.CourseDescription,
                Location = courseCreateVm.CourseLocation,
                StartingDate = courseCreateVm.CourseStartingDate,
                FinishingDate = courseCreateVm.CourseFinishingDate,
                Content = courseCreateVm.CourseContent,
                FutureJobDesc = courseCreateVm.FutureJobDescription,
                FutureJobSalary = courseCreateVm.FutureJobSalary,
                Properties = courseCreateVm.CourseProperties,
                Category = category,
                IsDrafted = false,
                CourseFragmentVideoName = uploadedVideo.fileName,
                CourseFragmentVideoPathOrContainer = uploadedVideo.pathOrContainerName,
                CourseImageName = uploadedImage.fileName,
                CourseImagePathOrContainer = uploadedImage.pathOrContainerName,
                CourseLevel = Enum.Parse<CourseLevels>(courseCreateVm.CourseLevel),
            };

            await _courseWriteRepository.AddAsync(course);
            await _courseWriteRepository.SaveAsync();

            if (courseCreateVm.FutureJobTitles != null && courseCreateVm.FutureJobTitles.Any())
            {
                List<FutureJobTitle> new_futureJobTitles = new List<FutureJobTitle>();

                foreach (var item in courseCreateVm.FutureJobTitles)
                {
                    FutureJobTitle futureJobTitle = new FutureJobTitle()
                    {
                        Content = item.Content,
                        Course = course
                    };

                    new_futureJobTitles.Add(futureJobTitle);
                }

                await _futureJobTitleWriteRepository.AddRangeAsync(new_futureJobTitles);
                await _futureJobTitleWriteRepository.SaveAsync();
            }

            if (courseCreateVm.Topics != null && courseCreateVm.Topics.Any())
            {
                List<Topic> new_topics = new List<Topic>();

                foreach (var item in courseCreateVm.Topics)
                {
                    Topic topic = new Topic()
                    {
                        Content = item.Content,
                        Course = course
                    };

                    new_topics.Add(topic);
                }

                await _topicWriteRepository.AddRangeAsync(new_topics);
                await _topicWriteRepository.SaveAsync();
            }

            return true;

        }
        public async Task<bool> UpdateCourseAsync(CourseUpdateVm courseUpdateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
                return false;


            Category? category = await _categoryReadRepository.GetByIdAsync(courseUpdateVm.CourseCategory);

            if (category == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(CourseUpdateVm.CourseCategory), "This Course Category Not Found");
            }

            Course? course = await _courseReadRepository.Table
                .Include(x => x.Category)
                .Include(x => x.FutureJobTitles)
                .Include(x => x.Topics)
                .FirstOrDefaultAsync(x => x.Id == courseUpdateVm.Id);

            if (course == null)
                return false;

            (string? fileName, string? pathOrContainerName) uploadedVideo = (null, null);

            if (courseUpdateVm.CourseFragmentVideo != null)
            {
                if (!string.IsNullOrEmpty(courseUpdateVm.CourseFragmentVideoName) && !string.IsNullOrEmpty(courseUpdateVm.CourseFragmentVideoPathOrContainer))
                {
                    await _storage.DeleteAsync(courseUpdateVm.CourseFragmentVideoPathOrContainer, courseUpdateVm.CourseFragmentVideoName);
                }

                FormFileCollection videos = new FormFileCollection
                {
                    courseUpdateVm.CourseFragmentVideo
                };

                List<(string fileName, string pathOrContainerName)> uploadedVideos = await _storage.UploadAsync(AzureContainerNames.CourseFragmentVideos, videos);

                uploadedVideo = uploadedVideos[0];
            }

            (string? fileName, string? pathOrContainerName) uploadedImage = (null, null);

            if (courseUpdateVm.CourseImage != null)
            {
                if (!string.IsNullOrEmpty(courseUpdateVm.CourseImageName) && !string.IsNullOrEmpty(courseUpdateVm.CourseImagePathOrContainer))
                {
                    await _storage.DeleteAsync(courseUpdateVm.CourseImagePathOrContainer, courseUpdateVm.CourseImageName);
                }

                FormFileCollection images = new FormFileCollection
                {
                    courseUpdateVm.CourseImage
                };

                List<(string fileName, string pathOrContainerName)> uploadedImages = await _storage.UploadAsync(AzureContainerNames.CourseCoverImages, images);

                uploadedImage = uploadedImages[0];
            }

            course.CourseCode = courseUpdateVm.CourseCode;
            course.Title = courseUpdateVm.CourseTitle;
            course.Heading = courseUpdateVm.CourseHeading;
            course.Description = courseUpdateVm.CourseDescription;
            course.HeadingDescription = courseUpdateVm.CourseHeaderDescription;
            course.Location = courseUpdateVm.CourseLocation;
            course.StartingDate = courseUpdateVm.CourseStartingDate;
            course.FinishingDate = courseUpdateVm.CourseFinishingDate;
            course.Content = courseUpdateVm.CourseContent;
            course.FutureJobDesc = courseUpdateVm.FutureJobDescription;
            course.FutureJobSalary = courseUpdateVm.FutureJobSalary;
            course.Properties = courseUpdateVm.CourseProperties;
            course.Category = category;
            course.IsDrafted = false;
            course.CourseLevel = Enum.Parse<CourseLevels>(courseUpdateVm.CourseLevel);

            if (!string.IsNullOrEmpty(uploadedVideo.fileName) && !string.IsNullOrEmpty(uploadedVideo.pathOrContainerName))
            {
                course.CourseFragmentVideoName = uploadedVideo.fileName;
                course.CourseFragmentVideoPathOrContainer = uploadedVideo.pathOrContainerName;
            }

            if (!string.IsNullOrEmpty(uploadedImage.fileName) && !string.IsNullOrEmpty(uploadedImage.pathOrContainerName))
            {
                course.CourseImageName = uploadedImage.fileName;
                course.CourseImagePathOrContainer = uploadedImage.pathOrContainerName;
            }


            _courseWriteRepository.Update(course);
            await _courseWriteRepository.SaveAsync();

            List<FutureJobTitle> futureJobTitles = await _futureJobTitleWriteRepository.Table
                .Include(x => x.Course)
                .Where(x => x.Course == course).ToListAsync();

            _futureJobTitleWriteRepository.RemoveRange(futureJobTitles);
            await _futureJobTitleWriteRepository.SaveAsync();


            if (courseUpdateVm.FutureJobTitles != null && courseUpdateVm.FutureJobTitles.Any())
            {
                List<FutureJobTitle> new_futureJobTitles = new List<FutureJobTitle>();

                foreach (var item in courseUpdateVm.FutureJobTitles)
                {
                    FutureJobTitle futureJobTitle = new FutureJobTitle()
                    {
                        Content = item.Content,
                        Course = course
                    };

                    new_futureJobTitles.Add(futureJobTitle);
                }

                await _futureJobTitleWriteRepository.AddRangeAsync(new_futureJobTitles);
                await _futureJobTitleWriteRepository.SaveAsync();
            }

            List<Topic> topics = await _topicWriteRepository.Table
                .Include(x => x.Course)
                .Where(x => x.Course == course).ToListAsync();

            _topicWriteRepository.RemoveRange(topics);
            await _topicWriteRepository.SaveAsync();

            if (courseUpdateVm.Topics != null && courseUpdateVm.Topics.Any())
            {
                List<Topic> new_Topics = new List<Topic>();

                foreach (var item in courseUpdateVm.Topics)
                {
                    Topic topic = new Topic()
                    {
                        Content = item.Content,
                        Course = course
                    };

                    new_Topics.Add(topic);
                }

                await _topicWriteRepository.AddRangeAsync(new_Topics);
                await _topicWriteRepository.SaveAsync();
            }

            return true;
        }

        public Task<IEnumerable<Course>> GetCoursesAsync()
        {
            IEnumerable<Course> courses = _courseReadRepository
                .GetWhere(x => !x.IsDeleted && !x.IsArchived)
                .Include(c => c.Instructors)
                .Include(c => c.Students)
                .Include(c => c.Sections);

            return Task.FromResult(courses);
        }

        public async Task<CourseSingleVm> GetCourseSingleAsync(Guid id)
        {
            Course? course = await _courseReadRepository.Table
                .Include(x => x.Instructors)
                .Include(x => x.Students)
                .Include(x => x.Tools)
                .Include(x => x.Topics)
                .Include(x => x.FutureJobTitles)
                .Include(x => x.Category)
                .Include(x => x.Prices)
                .ThenInclude(x => x.PriceInfos)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (course == null)
                return new CourseSingleVm();

            CourseSingleVm courseSingleVm = new()
            {
                Course = course,
                BaseUrl = _configuration["BaseUrl:Azure"],
            };
            return courseSingleVm;
        }

        public async Task<CourseUpdateVm> GetCourseUpdateDataAsync(Guid id)
        {
            Course? course = await _courseReadRepository.Table
                .Include(x => x.FutureJobTitles)
                .Include(x => x.Category)
                .Include(x => x.Topics)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (course == null)
                return new CourseUpdateVm();


            CourseUpdateVm courseUpdateVm = new CourseUpdateVm
            {
                Id = course.Id,
                CourseCategory = course.Category.Content,
                CourseContent = course.Content,
                CourseDescription = course.Description,
                CourseHeaderDescription = course.HeadingDescription,
                CourseFragmentVideo = null,
                CourseFragmentVideoName = course.CourseFragmentVideoName,
                CourseFragmentVideoPathOrContainer = course.CourseFragmentVideoPathOrContainer,
                CourseImage = null,
                CourseImageName = course.CourseImageName,
                CourseImagePathOrContainer = course.CourseImagePathOrContainer,
                CourseHeading = course.Heading,
                CourseLevel = course.CourseLevel.ToString(),
                CourseLocation = course.Location,
                CourseProperties = course.Properties,
                CourseStartingDate = course.StartingDate.Value,
                CourseFinishingDate = course.FinishingDate.HasValue ? course.FinishingDate.Value : null,
                CourseTitle = course.Title,
                FutureJobDescription = course.FutureJobDesc,
                FutureJobSalary = course.FutureJobSalary,
                CourseCode = course.CourseCode,
                FutureJobTitles = course.FutureJobTitles.Select(x => new FutureJobTitleVm
                {
                    Content = x.Content,
                }),
                Topics = course.Topics.Select(x => new TopicVm
                {
                    Content = x.Content
                }),
                BaseUrl = _configuration["BaseUrl:Azure"],
            };

            return courseUpdateVm;
        }

        public async Task<bool> RemoveCourseFragmentVideoAsync(Guid id)
        {
            Course? course = await _courseReadRepository.GetSingleAsync(x => x.Id == id);
            if (course == null)
                return false;

            await _storage.DeleteAsync(course.CourseFragmentVideoPathOrContainer, course.CourseFragmentVideoName);

            course.CourseFragmentVideoName = null;
            course.CourseFragmentVideoPathOrContainer = null;

            _courseWriteRepository.Update(course);
            await _courseWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteCourseAsync(Guid id)
        {
            Course? course = await _courseReadRepository.GetSingleAsync(x => x.Id == id);
            if (course == null)
                return false;

            course.IsDeleted = true;

            _courseWriteRepository.Update(course);
            await _courseWriteRepository.SaveAsync();
            return true;
        }


        #endregion

        #region Manage Instructors Services
        public async Task<CourseInstructorsVm> GetCourseInstructorsAsync(Guid id, string? searchFilter)
        {
            var course = await _courseReadRepository.Table
                .Include(x => x.Instructors)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (course == null) return new CourseInstructorsVm();

            var query = course.Instructors.AsQueryable();

            if (!string.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.ToLower();
                query = query.Where(x => x.UserName.ToLower().Contains(searchFilter) ||
                    x.FirstName.ToLower().Contains(searchFilter) ||
                    x.LastName.ToLower().Contains(searchFilter) ||
                    x.Email.ToLower().Contains(searchFilter));
            }

            var instructors = await query.ToListAsync();

            CourseInstructorsVm instructorsVm = new CourseInstructorsVm()
            {
                Instructors = instructors,
                Course = course,
                BaseUrl = _configuration["BaseUrl:Azure"]
            };

            return instructorsVm;
        }

        public async Task<bool> ReassignInstructorAsync(Guid courseId, Guid instructorId)
        {
            Course? course = await _courseReadRepository.Table
                .Include(x => x.Instructors)
                .FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null) return false;

            Instructor? instructor = await _instructorReadRepository.GetSingleAsync(x => x.Id == instructorId);
            if (instructor == null) return false;

            if (course.Instructors.Any(x => x.Id == instructor.Id))
            {
                List<DataAccess.Models.Instructor> instructors = course.Instructors.ToList();
                instructors.Remove(instructor);
                course.Instructors = instructors;
                _courseWriteRepository.Update(course);
                await _courseWriteRepository.SaveAsync();
            }

            return true;
        }

        public async Task<bool> AssignInstructorAsync(Guid courseId, Guid instructorId)
        {
            Course? course = await _courseReadRepository.Table
                .Include(x => x.Instructors)
                .FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null) return false;

            Instructor? instructor = await _instructorReadRepository.GetSingleAsync(x => x.Id == instructorId);
            if (instructor == null) return false;

            List<DataAccess.Models.Instructor> instructors = course.Instructors.ToList();
            instructors.Add(instructor);

            course.Instructors = instructors;

            _courseWriteRepository.Update(course);
            await _courseWriteRepository.SaveAsync();

            return true;
        }

        #endregion

        #region Manage Pricing Services

        public async Task<bool> AddCoursePriceAsync(CoursePriceCreateVm coursePriceCreateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
                return false;

            Course? course = await _courseReadRepository.GetSingleAsync(x => x.Id == coursePriceCreateVm.CourseId);

            if (course == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(CoursePriceCreateVm.CourseId), "Course Not Found");
                return false;
            }

            Price price = new Price()
            {
                Course = course,
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
            Course? course = await _courseReadRepository.Table
                .Include(x => x.Prices)
                .ThenInclude(x => x.PriceInfos)
                .Include(x => x.Prices)
                .ThenInclude(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (course == null)
                return new PaginationVm<IEnumerable<Price>>();

            List<Price> prices = course.Prices.ToList();

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
                CourseId = courseId,
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
    }
}
