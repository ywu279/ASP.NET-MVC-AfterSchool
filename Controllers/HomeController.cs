using AfterSchool.Models;
using AfterSchool.Models.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace AfterSchool.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AfterSchoolContext _context;

        public HomeController(ILogger<HomeController> logger, AfterSchoolContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var afterSchoolContext = _context.CourseOffers.Include(c => c.Course).Include(c => c.Location);
            return View(afterSchoolContext.ToList());
        }


        // GET: Home/Details/
        public async Task<IActionResult> Details(int cid, int lid, string sid)
        {
            if (cid == null || lid == null || sid == null || _context.CourseOffers == null)
            {
                return NotFound();
            }

            var courseOffer = await _context.CourseOffers
                .Include(c => c.Course)
                .Include(c => c.Location)
                .FirstOrDefaultAsync(m => m.CourseId == cid && m.LocationId == lid && m.StartDate == DateTime.Parse(sid));
            if (courseOffer == null)
            {
                return NotFound();
            }

            var instructors = (from t in _context.TeachingRecords
                               join i in _context.Instructors on t.InstructorId equals i.Id
                               join c in _context.CourseOffers on new { t.CourseId, t.LocationId, t.StartDate } equals new { c.CourseId, c.LocationId, c.StartDate }
                               where t.CourseId == cid && t.LocationId == lid && t.StartDate == DateTime.Parse(sid)
                               select i);
            courseOffer.Instructors = instructors.ToList();

            return View(courseOffer);
        }


        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}