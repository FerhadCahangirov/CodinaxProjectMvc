using CodinaxProjectMvc.Context;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.Filters;
using CodinaxProjectMvc.ViewModel.LayoutVm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CodinaxProjectMvc.Controllers
{
    [CurrentLangFilterFactory]
    public class AboutController : Controller
    {
        private readonly CodinaxDbContext _db;
        private readonly IConfiguration _configuration;

        public AboutController(CodinaxDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            List<Feature>? features = await _db.Features.Where(x => !x.IsDeleted && !x.IsArchived).ToListAsync();
            AboutVm aboutVm = new AboutVm() 
            { 
                Features = features,
                BaseUrl = _configuration["BaseUrl:Azure"]
            };
            return View(aboutVm);
        }
    }
}
