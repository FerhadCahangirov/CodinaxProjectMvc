﻿using CodinaxProjectMvc.Business.Abstract.InfrastructureServices;
using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using CodinaxProjectMvc.DataList;
using CodinaxProjectMvc.ViewModel.InstructorVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CodinaxProjectMvc.Areas.Auth.Controllers
{
    [Area("Auth")]
    [Authorize(Policy = PolicyConstants.NotAuthRequiredPolicy)]
    public class InstructorController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAuthService _authService;

        public InstructorController(UserManager<AppUser> userManager, IAuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }

        public IActionResult Apply()
        {
            ViewBag.Countries = new SelectList(Countries.All, nameof(Country.Name), nameof(Country.Name));
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Apply(InstructorApplyVm instructorApplyVm)
        {
            bool result = await _authService.InstructorApplyAsync(instructorApplyVm);
                
            if (!result)
            {
                ViewBag.Countries = new SelectList(Countries.All, nameof(Country.Name), nameof(Country.Name));
                return View(instructorApplyVm);
            }

            TempData["InstructorApplySuccess"] = true;
            return Redirect("/Home/Index");
        }
    }
}
