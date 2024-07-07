using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CodinaxProjectMvc.ViewComponents.AboutViewComponents
{
    public class AboutViewComponent : ViewComponent
    {
        private readonly IReadRepository<About> _aboutReadRepository;

        public AboutViewComponent(IReadRepository<About> aboutReadRepository)
        {
            _aboutReadRepository = aboutReadRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
            => View(await _aboutReadRepository
                .GetWhere(x => !x.IsDeleted && !x.IsArchived).ToListAsync()
                );
    }
}
