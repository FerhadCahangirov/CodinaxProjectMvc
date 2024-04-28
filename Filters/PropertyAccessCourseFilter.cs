using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CodinaxProjectMvc.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PropertyAccessCourseFilterFactoryAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetRequiredService<PropertyAccessCourseFilter>();
            return filter;
        }
    }

    public class PropertyAccessCourseFilter : IAsyncActionFilter
    {
        private readonly IReadRepository<Course> _courseReadRepository;
        private readonly IReadRepository<Category> _categoryReadRepository;
        private readonly IReadRepository<Template> _templateReadRepository;

        public PropertyAccessCourseFilter(IReadRepository<Course> courseReadRepository, IReadRepository<Category> categoryReadRepository, IReadRepository<Template> templateReadRepository)
        {
            _courseReadRepository = courseReadRepository;
            _categoryReadRepository = categoryReadRepository;
            _templateReadRepository = templateReadRepository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Controller? controller = context.Controller as Controller;

            if(controller != null)
            {
                controller.ViewBag.Categories = new SelectList(_categoryReadRepository.GetWhere(x => !x.IsDeleted && !x.IsArchived), nameof(Category.Id), nameof(Category.Content));

                controller.ViewBag.Templates = new SelectList(_templateReadRepository.GetWhere(x => !x.IsDeleted && !x.IsArchived), nameof(Template.Id), nameof(Template.Heading));

                var courses = _courseReadRepository.GetWhere(x => !x.IsDeleted && !x.IsArchived);

                if (courses != null)
                {
                    controller.ViewBag.Courses = new SelectList(courses, nameof(Course.Id), nameof(Course.Title));
                }

                var courseLevels = Enum.GetValues(typeof(CourseLevels));

                var selectListItems = new List<SelectListItem>();

                foreach (var value in courseLevels)
                {
                    selectListItems.Add(new SelectListItem
                    {
                        Text = Enum.GetName(typeof(CourseLevels), value),
                        Value = ((int)value).ToString()
                    });
                }

                controller.ViewBag.CourseLevels = new SelectList(selectListItems, "Value", "Text");

            }

            await next();
        }
    }
}
