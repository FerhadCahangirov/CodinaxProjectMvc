using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CodinaxProjectMvc.Filters
{

    [AttributeUsage(AttributeTargets.Class)]
    public class CurrentLangFilterFactoryAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            CurrentLangFilter filter = serviceProvider.GetRequiredService<CurrentLangFilter>();

            return filter;
        }
    }

    public class CurrentLangFilter : IAsyncActionFilter
    {
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var rqf = context.HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = rqf.RequestCulture.Culture;

            var controller = context.Controller as Controller;

            if (controller != null)
            {
                controller.ViewData["Culture"] = culture;

                if(culture.ToString().ToLower() == "ru-ru")
                {
                    controller.ViewData["Img"] = "/Assets/imgs/vectors/russia.png";
                    controller.ViewData["Txt"] = "Russian";
                }
                else if(culture.ToString().ToLower() == "tr-tr")
                {
                    controller.ViewData["Img"] = "/Assets/imgs/vectors/turkey.png";
                    controller.ViewData["Txt"] = "Turkish";
                }
                else
                {
                    controller.ViewData["Img"] = "/Assets/imgs/vectors/united-kingdom.png";
                    controller.ViewData["Txt"] = "English";
                }

            }

            return next();
        }
    }
}
