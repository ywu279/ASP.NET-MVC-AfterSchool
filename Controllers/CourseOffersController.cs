using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AfterSchool.Models.DataAccess;

namespace AfterSchool.Controllers
{
    public class CourseOffersController : Controller
    {
        private readonly AfterSchoolContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CourseOffersController(AfterSchoolContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: CourseOffers
        public async Task<IActionResult> Index()
        {
            var afterSchoolContext = _context.CourseOffers.Include(c => c.Course).Include(c => c.Location);
            return View(await afterSchoolContext.ToListAsync());
        }

        // GET: CourseOffers/Details/5
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

            return View(courseOffer);
        }



        // GET: CourseOffers/Create
        public IActionResult Create()
        {
            
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "FullCourseName");
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name");
            return View();
        }


        // POST: CourseOffers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,LocationId,StartDate,EndDate,Price,ImageName,ImageFile")] CourseOffer courseOffer)
        {
            if (_context.CourseOffers.Any(e => e.CourseId == courseOffer.CourseId && e.LocationId == courseOffer.LocationId && e.StartDate == courseOffer.StartDate))
            {
                ModelState.AddModelError("", "This course offer already exists!");
            }
            if (courseOffer.EndDate < courseOffer.StartDate)
            {
                ModelState.AddModelError("EndDate", "End date must be greater than or equal to Start Date!");
            }

            if (ModelState.IsValid)
            {
                // Save image to wwwroot/Images
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(courseOffer.ImageFile.FileName);
                string extension = Path.GetExtension(courseOffer.ImageFile.FileName);
                courseOffer.ImageName = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/CourseOffers/", courseOffer.ImageName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await courseOffer.ImageFile.CopyToAsync(fileStream);
                }

                _context.Add(courseOffer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "FullCourseName", courseOffer.CourseId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", courseOffer.LocationId);
            return View(courseOffer);
        }



        // GET: CourseOffers/Edit/5
        public async Task<IActionResult> Edit(int cid, int lid, string sid)
        {
            if (cid == null || lid == null || sid == null || _context.CourseOffers == null)
            {
                return NotFound();
            }

            //var courseOffer = await _context.CourseOffers.FindAsync(id);
            var courseOffer = await _context.CourseOffers
                .Include(c => c.Course)
                .Include(c => c.Location)
                .FirstOrDefaultAsync(m => m.CourseId == cid && m.LocationId == lid && m.StartDate == DateTime.Parse(sid));

            if (courseOffer == null)
            {
                return NotFound();
            }

            //ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", courseOffer.CourseId);
            //ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id", courseOffer.LocationId);
            return View(courseOffer);
        }


        // POST: CourseOffers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("CourseId,LocationId,StartDate,EndDate,Price,ImageName,ImageFile")] CourseOffer courseOffer)
        {
            if (courseOffer.EndDate < courseOffer.StartDate)
            {
                ModelState.AddModelError("EndDate", "End date must be greater than or equal to Start Date!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (courseOffer.ImageFile != null)
                    {
                        //delete old image from wwwroot/Images
                        if (courseOffer.ImageName != null)
                        {
                            string path = Path.Combine(_hostEnvironment.WebRootPath, "Images", "CourseOffers", courseOffer.ImageName);
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                        }
                        // Save new image to wwwroot/Images
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(courseOffer.ImageFile.FileName);
                        string extension = Path.GetExtension(courseOffer.ImageFile.FileName);
                        courseOffer.ImageName = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        string newPath = Path.Combine(wwwRootPath + "/Images/CourseOffers/", courseOffer.ImageName);
                        using (var fileStream = new FileStream(newPath, FileMode.Create))
                        {
                            await courseOffer.ImageFile.CopyToAsync(fileStream);
                        }
                    }

                    _context.Update(courseOffer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseOfferExists(courseOffer.CourseId) || !CourseOfferExists(courseOffer.LocationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", courseOffer.CourseId);
            //ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id", courseOffer.LocationId);
            return View(courseOffer);
        }



        // GET: CourseOffers/Delete/5
        public async Task<IActionResult> Delete(int cid, int lid, string sid)
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

            return View(courseOffer);
        }


        // POST: CourseOffers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("CourseId,LocationId,StartDate,EndDate,Price,ImageName,ImageFile")] CourseOffer courseOffer)
        {
            if (_context.CourseOffers == null)
            {
                return Problem("Entity set 'AfterSchoolContext.CourseOffers'  is null.");
            }
            
            //delete image from wwwroot/Images
            if (courseOffer.ImageName != null)
            {
                string path = Path.Combine(_hostEnvironment.WebRootPath, "Images", "CourseOffers", courseOffer.ImageName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            //delete the record
            if (courseOffer != null)
            {
                _context.CourseOffers.Remove(courseOffer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        private bool CourseOfferExists(int id)
        {
          return (_context.CourseOffers?.Any(e => e.CourseId == id)).GetValueOrDefault();
        }
    }
}
