using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cidean.GatherHub.Models;
using Cidean.GatherHub.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Cidean.GatherHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly HubContext _context;

        public HomeController(HubContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            return View(_context.Courses.Include(course => course.Category).ToList());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
