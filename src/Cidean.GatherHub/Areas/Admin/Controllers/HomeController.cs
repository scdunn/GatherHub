using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cidean.GatherHub.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cidean.GatherHub.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(AuthenticationSchemes = "admin")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _work;

        public HomeController(IUnitOfWork work)
        {
            _work = work;
        }

        public IActionResult Index()
        {
            var activityLogItems = _work.ActivityLogItems.GetAll();
            return View(activityLogItems);
        }
    }
}