using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.ViewComponents.ApplyStepsViewComponents
{
    public class ApplyStepsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
            => View();
    }
}
