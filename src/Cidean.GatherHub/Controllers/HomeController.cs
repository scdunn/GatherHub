using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Cidean.GatherHub.Models;
using Cidean.GatherHub.Core.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
            return View(_work.Courses.GetAll()
                .Include(course => course.Category)
                .Include(course => course.Instructor)
                .Include(course => course.CourseMembers)
                .ThenInclude(m => m.Member));
                

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
        public async Task<IActionResult> Course(int? id)
        {
            if (id == null)
                return NotFound();


            var course = await _work.Courses.GetAll()
                .Include(p => p.Category)
                .Include(p => p.Instructor)
                .Include(p => p.CourseMembers)
                .ThenInclude(p => p.Member)
                .SingleAsync(p=>p.Id==id.Value);

            return View(course);
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
