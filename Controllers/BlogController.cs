﻿using Microsoft.AspNetCore.Mvc;

namespace CodinaxProjectMvc.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
