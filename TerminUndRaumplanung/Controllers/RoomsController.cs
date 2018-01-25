using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppData;
using AppData.Models;
using Microsoft.AspNetCore.Authorization;

// Erstellt durch Maximilian Freiberger
namespace TerminUndRaumplanung.Controllers
{

    /// <summary>
    /// Rooms Controller
    /// </summary>
    public class RoomsController : Controller
    {
        private readonly AppointmentContext _context;
        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }



        /// <summary>
        /// Constructor for Rooms Controller
        /// </summary>
        /// <param name="context"></param>
        public RoomsController(AppointmentContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: Rooms
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rooms.ToListAsync());
        }



        /// <summary>
        /// GET: Rooms/Details/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                //.Include(m => m.BookedTimes)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }



        /// <summary>
        /// GET: Rooms/Create
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }



        /// <summary>
        /// POST: Rooms/Create
        /// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        /// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("NumberOfSeats,BeamerIsAvailable,SpeakerIsAvailable,Id,Name")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }



        /// <summary>
        /// GET: Rooms/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.SingleOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }



        /// <summary>
        /// POST: Rooms/Edit/5
        /// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        /// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="room"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("NumberOfSeats,BeamerIsAvailable,SpeakerIsAvailable,Id,Name")] Room room)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
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
            return View(room);
        }



        /// <summary>
        /// GET: Rooms/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .SingleOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }



        /// <summary>
        /// POST: Rooms/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.SingleOrDefaultAsync(m => m.Id == id);
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
