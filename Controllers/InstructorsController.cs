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
    public class InstructorsController : Controller
    {
        private readonly AfterSchoolContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public InstructorsController(AfterSchoolContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Instructors
        public async Task<IActionResult> Index()
        {
              return _context.Instructors != null ? 
                          View(await _context.Instructors.ToListAsync()) :
                          Problem("Entity set 'AfterSchoolContext.Instructors'  is null.");
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Instructors == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }



        // GET: Instructors/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Instructors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Description,ImageName,ImageFile")] Instructor instructor)
        {
            // Server-side validation
            if (instructor.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The Upload Image field is required.");
            }

            if (ModelState.IsValid)
            {
                // Save image to wwwroot/Images
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(instructor.ImageFile.FileName);
                string extension = Path.GetExtension(instructor.ImageFile.FileName);
                instructor.ImageName = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/Instructors/", instructor.ImageName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await instructor.ImageFile.CopyToAsync(fileStream);
                }
                //Insert record
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }



        // GET: Instructors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Instructors == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Description,ImageName,ImageFile")] Instructor instructor)
        {
            if (id != instructor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {  
                    if(instructor.ImageFile != null)
                    {
                        //delete image from wwwroot/Images
                        if(instructor.ImageName != null)
                        {
                            string path = Path.Combine(_hostEnvironment.WebRootPath, "Images", "Instructors", instructor.ImageName);
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                        }
                        // Save new image to wwwroot/Images
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(instructor.ImageFile.FileName);
                        string extension = Path.GetExtension(instructor.ImageFile.FileName);
                        instructor.ImageName = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        string newPath = Path.Combine(wwwRootPath + "/Images/Instructors/", instructor.ImageName);
                        using (var fileStream = new FileStream(newPath, FileMode.Create))
                        {
                            await instructor.ImageFile.CopyToAsync(fileStream);
                        }
                    }

                    //update the record
                    _context.Update(instructor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.Id))
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
            return View(instructor);
        }



        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Instructors == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }


        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Instructors == null)
            {
                return Problem("Entity set 'AfterSchoolContext.Instructors'  is null.");
            }
            var instructor = await _context.Instructors.FindAsync(id);

            //delete image from wwwroot/Images
            if (instructor.ImageName != null)
            {
                string path = Path.Combine(_hostEnvironment.WebRootPath, "Images", "Instructors", instructor.ImageName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            //delete the record
            if (instructor != null)
            {
                _context.Instructors.Remove(instructor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
          return (_context.Instructors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
