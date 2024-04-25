using CodinaxProjectMvc.Context;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel.InstructorVm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly CodinaxDbContext _db;
        private readonly IConfiguration _configuration;

        public InstructorsController(CodinaxDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Instructor> instructors = await _db.Instructors
                .Where(x => !x.IsBanned && x.IsApproved).ToListAsync();

            IEnumerable<Faq> faqs = await _db.Faqs.Where(x => !x.IsDeleted && !x.IsArchived).ToListAsync();

            InstructorsVm instructorsVm = new InstructorsVm() 
            { 
                Instructors = instructors,
                Faqs = faqs,
                BaseUrl = _configuration["BaseUrl:Azure"],
            };

            return View(instructorsVm);
        }
    }
}
