using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cidean.GatherHub.Models;
using Cidean.GatherHub.Core.Data;

namespace Cidean.GatherHub.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var context = new HubContext();
            return View(context.Courses.ToList());
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
