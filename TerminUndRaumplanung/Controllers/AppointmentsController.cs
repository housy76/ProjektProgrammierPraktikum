using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppData;
using AppData.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Collections.ObjectModel;

namespace TerminUndRaumplanung.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly AppointmentContext _context;
        public IEnumerable<SelectListItem> RessourceList { get; set; }

        public AppointmentsController(AppointmentContext context)
        {
            _context = context;
        }

        // GET: Appointments
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            return View(await _context
                .Appointments
                .Include(a => a.Room)
                .Include(a => a.Ressources)
                .ToListAsync());
        }

        // GET: Appointments/Details/5
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context
                .Appointments
                .Include(a => a.Ressources)
                .Include(a => a.Room)
                .Include(a => a.Survey)
                .OrderByDescending(a => a.StartTime)
                //.Where(a => a.StartTime > System.DateTime.Now.AddDays(-1))
                .SingleOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }


        [Authorize(Roles = "Administrator,User")]
        public IActionResult Create(int surveyId)
        {
            var model = new Appointment
            {
                StartTime = System.DateTime.Now,
                EndTime = System.DateTime.Now.AddHours(1),
                Survey = _context
                    .Surveys
                    .SingleOrDefault(s => s.Id == surveyId),
                Ressources = _context
                    .Ressources
                    .OrderBy(r => r.Name)
                    .ToList()
            };

            ViewBag.RoomList = _context
                .Rooms
                .OrderBy(r => r.Name)
                .ToList();

            return View(model);
        }



        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to 
        // bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> Create(
            [Bind("Id,StartTime,EndTime,SelectedRoom,SelectedRessource,Survey")] Appointment appointment)
        {
            //get related Survey object from database and store it into the Appointment object
            appointment.Survey = _context
                                    .Surveys
                                    .Include(s => s.Creator)
                                    .FirstOrDefault(s => s.Id == appointment.Survey.Id);

            //get selected Room Object from database and store it into the Appointment Object
            appointment.Room = (Room)_context
                .Ressources
                .Include(r => r.RessourceBookedTimes)
                    .ThenInclude(s => s.Ressource)
                .Include(r => r.RessourceBookedTimes)
                    .ThenInclude(s => s.BookedTime)
                .SingleOrDefault(r => r.Id == appointment.SelectedRoom);

            //create bookedTime for booking room and ressources
            var bookedTime = new BookedTime
            {
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
            };

            var ressourceBookedTime = new RessourceBookedTime
            {
                BookedTime = bookedTime,
                BookedTimeId = bookedTime.Id,

                Ressource = appointment.Room,
                RessourceId = appointment.Room.Id
            };


            bookedTime.RessourcesBookedTimes = new List<RessourceBookedTime>
            {
                ressourceBookedTime
            };

            //add the time to the booked room
            if (appointment.Room.RessourceBookedTimes == null)
            {
                appointment.Room.RessourceBookedTimes = new List<RessourceBookedTime>();
            }
            appointment.Room.RessourceBookedTimes.Add(ressourceBookedTime);


            //get selected Ressource Objects from database and store it into this Appointment
            if (appointment.SelectedRessource != null)
            {
                //get selected Ressource Objects from database and store them into this Appointment
                appointment.Ressources = new Collection<Ressource>();

                foreach (var item in appointment.SelectedRessource)
                {
                    var ressource = _context
                            .Ressources
                            .Include(r => r.RessourceBookedTimes)
                            .SingleOrDefault(r => r.Id == item);

                    var rbt = new RessourceBookedTime
                    {
                        BookedTime = bookedTime,
                        BookedTimeId = bookedTime.Id,

                        Ressource = appointment.Room,
                        RessourceId = appointment.Room.Id
                    };

                    bookedTime.RessourcesBookedTimes.Add(rbt);

                    ressource.RessourceBookedTimes.Add(rbt);
                    
                }

            }

            //survey is explicitly loaded. SelectedMember is a direct entity. This
            //entity will never be saved into database. Everytime it's null.
            //It's a requuired entity for the survey and is also checked during 
            //appointment validation. Without this line it will always be unvalidated!
            appointment.Survey.SelectedMember = new Collection<string>();
            
            //After model binding and validation are complete, you may want to repeat parts
            //of it. For example, a user may have entered text in a field expecting an 
            //integer, or you may need to compute a value for a model's property.
            ModelState.Clear();
            TryValidateModel(appointment);

            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                _context.Add(bookedTime);
                _context.Add(ressourceBookedTime);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Surveys", new { id = appointment.Survey.Id });
            }
            else
            {
                appointment.Ressources = _context.Ressources.ToList();
                ViewBag.RoomList = _context
                    .Rooms
                    .OrderBy(r => r.Name)
                    .ToList();
            }
            return View(appointment);
        }



        // GET: Appointments/Edit/5
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Simon
            var appointment = await _context.Appointments
                .Include(a => a.Survey)
                .Include(a => a.Room)
                .Include(a => a.Ressources)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            ViewBag.RoomList = _context
                .Rooms
                .OrderBy(r => r.Name)
                .ToList();

            ViewBag.RessourceList = _context
                .Ressources
                .OrderBy(r => r.Name)
                .ToList();

            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> Edit(int id,
                    [Bind("Id,StartTime,EndTime,SelectedRessource,SelectedRoom")] Appointment appointment
            )
        {

            if (id != appointment.Id)
            {
                return NotFound();
            }


            //create new bookedTime for booking room and ressources
            var newBookedTime = new BookedTime
            {
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
            };

            appointment.Room = _context
                .Rooms
                //.Include(r => r.BookedTimes)
                .SingleOrDefault(r => r.Id == appointment.SelectedRoom);

            //add the time to the booked room
            //if (appointment.Room.BookedTimes == null)
            //{
            //    appointment.Room.BookedTimes = new Collection<BookedTime>();
            //}
            //appointment.Room.BookedTimes.Add(newBookedTime);

            //get selected Ressource Objects from database and store it into this Appointment
            if (appointment.SelectedRessource != null)
            {
                //get selected Ressource Objects from database and store them into this Appointment
                appointment.Ressources = new Collection<Ressource>();

                //foreach (var item in appointment.SelectedRessource)
                //{
                //    appointment.Ressources.Add(
                //        _context
                //            .Ressources
                //            .Include(r => r.BookedTimes)
                //            .SingleOrDefault(r => r.Id == item)
                //    );
                //}

                //foreach (var item in appointment.Ressources)
                //{
                //    if (item.BookedTimes == null)
                //    {
                //        item.BookedTimes = new Collection<BookedTime>();
                //    }
                //    item.BookedTimes.Add(newBookedTime);
                //}
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(
                    "Details",  //controller action
                    "Surveys",   //controller

                    //Man muss ein neues Objekt erzeugen, das wie die Variable der
                    //empfangenden Methode des Controllers heißt.
                    //Das Übergeben der id als direkte INT - Zahl funktioniert nicht.
                    new { id = appointment.Survey.Id });
            }
            return View(appointment);
        }



        //POST: Surveys/UpdateRessourceList
        //adds an additional User to the existing member list of the survey
        [HttpPost]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> UpdateRessourceList(
            [Bind("Id,Ressource")] Appointment appointment,
            int selectedRessource)
        {


            appointment = _context
                .Appointments
                .Include(a => a.Survey)
                .Include(a => a.Room)
                .Include(a => a.Ressources)
                .SingleOrDefault(a => a.Id == appointment.Id);

            var newRessource = _context
                    .Ressources
                    //.Include(r => r.BookedTimes)
                    .SingleOrDefault(r => r.Id == selectedRessource);

            var bookedTime = new BookedTime()
            {
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime
            };

            
            //add bookedTime to new Ressource to book the new ressource!
            //newRessource.BookedTimes.Add(bookedTime);

            appointment.Ressources.Add(newRessource);


            _context.Update(appointment);
            _context.Update(newRessource);
            await _context.SaveChangesAsync();


            //create room list for view to be passed
            ViewBag.RoomList = _context
                .Rooms
                .OrderBy(r => r.Name)
                .ToList();

            ViewBag.RessourceList = _context
                .Ressources
                .OrderBy(r => r.Name)
                .ToList();


            return RedirectToAction("Edit", appointment);
        }



        [Authorize(Roles = "Administrator,User")]
        [Route("Appointments/RemoveRessource/{ressourceId},{appointmentId}")] //attribute routing for specific url parameters
        public async Task<IActionResult> RemoveRessource(int ressourceId, int appointmentId)
        {
            
            var appointment = _context
                .Appointments
                .Include(a => a.Survey)
                .Include(a => a.Room)
                .Include(a => a.Ressources)
                    //.ThenInclude(r => r.BookedTimes)
                .SingleOrDefault(a => a.Id == appointmentId);

            var deletedRessource = _context
                .Ressources
                //.Include(r => r.BookedTimes)
                .SingleOrDefault(r => r.Id == ressourceId);

            var bookedTime = _context
                .BookedTimes
                .FirstOrDefault(b => b.StartTime == appointment.StartTime && b.EndTime == appointment.EndTime);

            //var x = 45;

            //remove booked time from ressource that it is again available in this time
            //deletedRessource.BookedTimes.Remove(bookedTime);

            //remove ressource from appointment
            appointment.Ressources.Remove(deletedRessource);

            _context.Update(appointment);
            _context.Update(deletedRessource);
            await _context.SaveChangesAsync();


            //create room list for view to be passed
            ViewBag.RoomList = _context
                .Rooms
                .OrderBy(r => r.Name)
                .ToList();

            return RedirectToAction("Edit", appointment);
        }


        


        // GET: Appointments/Delete/5
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .SingleOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.SingleOrDefaultAsync(m => m.Id == id);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator,User")]
        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
