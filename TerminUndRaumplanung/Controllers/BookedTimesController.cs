using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppData;
using AppData.Models;
using Microsoft.AspNetCore.Authorization;

namespace TerminUndRaumplanung.Controllers
{
    public class BookedTimesController : Controller
    {
        private readonly AppointmentContext _context;

        public BookedTimesController(AppointmentContext context)
        {
            _context = context;
        }

        // GET: BookedTimes
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            return View(await _context
                .BookedTimes
                .Include(b => b.Ressource)
                .ToListAsync()
                );
        }

        // GET: BookedTimes/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookedTime = await _context.BookedTimes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (bookedTime == null)
            {
                return NotFound();
            }

            return View(bookedTime);
        }

        // GET: BookedTimes/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: BookedTimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,StartTime,EndTime")] BookedTime bookedTime)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookedTime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookedTime);
        }

        // GET: BookedTimes/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookedTime = await _context.BookedTimes.SingleOrDefaultAsync(m => m.Id == id);
            if (bookedTime == null)
            {
                return NotFound();
            }
            return View(bookedTime);
        }

        // POST: BookedTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,EndTime")] BookedTime bookedTime)
        {
            if (id != bookedTime.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookedTime);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookedTimeExists(bookedTime.Id))
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
            return View(bookedTime);
        }

        // GET: BookedTimes/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookedTime = await _context.BookedTimes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (bookedTime == null)
            {
                return NotFound();
            }

            return View(bookedTime);
        }

        // POST: BookedTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookedTime = await _context.BookedTimes.SingleOrDefaultAsync(m => m.Id == id);
            _context.BookedTimes.Remove(bookedTime);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        private bool BookedTimeExists(int id)
        {
            return _context.BookedTimes.Any(e => e.Id == id);
        }
    }
}
