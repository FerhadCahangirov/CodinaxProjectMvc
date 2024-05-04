using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.ViewComponents.SubscribeViewComponents
{
    public class SubscribeViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
