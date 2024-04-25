using CodinaxProjectMvc.Context;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel.LayoutVm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CodinaxProjectMvc.Controllers
{
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
            List<About>? abouts = await _db.Abouts.Where(x => !x.IsDeleted && !x.IsArchived).ToListAsync();
            List<Feature>? features = await _db.Features.Where(x => !x.IsDeleted && !x.IsArchived).ToListAsync();
            List<Faq>? faqs = await _db.Faqs.Where(x => !x.IsDeleted && !x.IsArchived).ToListAsync();
            AboutVm aboutVm = new AboutVm() 
            { 
                Abouts = abouts,
                Features = features,
                Faqs = faqs,
                BaseUrl = _configuration["BaseUrl:Azure"]
            };
            return View(aboutVm);
        }
    }
}
