using CodinaxProjectMvc.Business.Abstract.InfrastructureServices;
using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Business.InfrastructureServices;
using CodinaxProjectMvc.Business.PersistenceServices;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.DataAccess.Repositories;
using System.Reflection;

namespace CodinaxProjectMvc.Registrations
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IInstructorService, InstructorService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IFeatureService, FeatureService>();
            services.AddScoped<IAboutService, AboutService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IFaqService, FaqService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<ISubscriberService, SubscriberService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<ILectureService, LectureService>();
            services.AddScoped<IBookmarkService, BookmarkService>();

            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
            return services;
        }
    }
}
