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
    public class TeachingRecordsController : Controller
    {
        private readonly AfterSchoolContext _context;

        public TeachingRecordsController(AfterSchoolContext context)
        {
            _context = context;
        }

        // GET: TeachingRecords
        public async Task<IActionResult> Index()
        {
            var afterSchoolContext = _context.TeachingRecords.Include(t => t.CourseOffer).Include(t => t.Instructor);
            return View(await afterSchoolContext.ToListAsync());
        }

        // GET: TeachingRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TeachingRecords == null)
            {
                return NotFound();
            }

            var teachingRecord = await _context.TeachingRecords
                .Include(t => t.CourseOffer)
                .Include(t => t.Instructor)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (teachingRecord == null)
            {
                return NotFound();
            }

            return View(teachingRecord);
        }

        // GET: TeachingRecords/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.CourseOffers, "CourseId", "CourseId");
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "FirstName");
            return View();
        }

        // POST: TeachingRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,LocationId,StartDate,InstructorId,Timestamp")] TeachingRecord teachingRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teachingRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.CourseOffers, "CourseId", "CourseId", teachingRecord.CourseId);
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "FirstName", teachingRecord.InstructorId);
            return View(teachingRecord);
        }

        // GET: TeachingRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TeachingRecords == null)
            {
                return NotFound();
            }

            var teachingRecord = await _context.TeachingRecords.FindAsync(id);
            if (teachingRecord == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.CourseOffers, "CourseId", "CourseId", teachingRecord.CourseId);
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "FirstName", teachingRecord.InstructorId);
            return View(teachingRecord);
        }

        // POST: TeachingRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,LocationId,StartDate,InstructorId,Timestamp")] TeachingRecord teachingRecord)
        {
            if (id != teachingRecord.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teachingRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeachingRecordExists(teachingRecord.CourseId))
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
            ViewData["CourseId"] = new SelectList(_context.CourseOffers, "CourseId", "CourseId", teachingRecord.CourseId);
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "FirstName", teachingRecord.InstructorId);
            return View(teachingRecord);
        }

        // GET: TeachingRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TeachingRecords == null)
            {
                return NotFound();
            }

            var teachingRecord = await _context.TeachingRecords
                .Include(t => t.CourseOffer)
                .Include(t => t.Instructor)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (teachingRecord == null)
            {
                return NotFound();
            }

            return View(teachingRecord);
        }

        // POST: TeachingRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TeachingRecords == null)
            {
                return Problem("Entity set 'AfterSchoolContext.TeachingRecords'  is null.");
            }
            var teachingRecord = await _context.TeachingRecords.FindAsync(id);
            if (teachingRecord != null)
            {
                _context.TeachingRecords.Remove(teachingRecord);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeachingRecordExists(int id)
        {
          return _context.TeachingRecords.Any(e => e.CourseId == id);
        }
    }
}
