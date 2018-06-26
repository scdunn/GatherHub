using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cidean.GatherHub.Models;
using Cidean.GatherHub.Core.Data;
using Microsoft.EntityFrameworkCore;
using Cidean.GatherHub.Core.Models;

namespace Cidean.GatherHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _work;

        public HomeController(IUnitOfWork work)
        {
            _work = work;
        }

        public IActionResult Index()
        {
            return View(_work.Courses.GetAll().Include(course => course.Category));
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
