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
using TerminUndRaumplanung.Models;

namespace TerminUndRaumplanung.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AppointmentsController : Controller
    {
        
        private readonly AppointmentContext _context;

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<SelectListItem> RessourceList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public AppointmentsController(AppointmentContext context)
        {
            _context = context;
        }



        /// <summary>
        /// GET: Appointments
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            //create view model object
            var model = new List<AppointmentViewModel>();

            //get all appointments form DB
            var allAppointments = await _context
                .Appointments
                .Include(a => a.Room)
                .Include(a => a.AppointmentRessources)
                    .ThenInclude(ar => ar.Ressource)
                .ToListAsync();

            //set entities of view model
            foreach (var app in allAppointments)
            {
                var appointment = new AppointmentViewModel
                {
                    AppointmentId = app.Id,
                    StartTime = app.StartTime,
                    EndTime = app.EndTime,
                    Room = app.Room,
                    Ressources = new List<Ressource>()
                };

                //get all ressources for this appointment and set them into view model
                foreach (var ar in app.AppointmentRessources)
                {
                    var res = ar.Ressource;
                    appointment.Ressources.Add(res);
                }

                model.Add(appointment);
            }

            return View(model);
        }



        ///// <summary>
        ///// GET: Appointments/Details/5
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[Authorize(Roles = "Administrator,User")]
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var appointment = await _context
        //        .Appointments
        //        .Include(a => a.AppointmentRessources)
        //        .Include(a => a.Room)
        //        .Include(a => a.Survey)
        //        .OrderByDescending(a => a.StartTime)
        //        //.Where(a => a.StartTime > System.DateTime.Now.AddDays(-1))
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (appointment == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(appointment);
        //}



        /// <summary>
        /// 
        /// </summary>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator,User")]
        public IActionResult Create(int surveyId)
        {
            var model = new AppointmentViewModel
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



        /// <summary>
        /// POST: Appointments/Create
        /// To protect from overposting attacks, please enable the specific properties you want to 
        /// bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// </summary>
        /// <param name="appointmentModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> Create(
            [Bind("Id,StartTime,EndTime,SelectedRoom,SelectedRessource,Survey")] AppointmentViewModel appointmentModel)
        {

            //test if there is an appointment with the same room and Time
            //if yes, appointment can not be created to avoid dublicates
            var existingAppointment = _context
                .Appointments
                .Include(a => a.Room)
                .Include(a => a.Survey)
                .FirstOrDefault(a => a.Survey.Id == appointmentModel.Survey.Id &&
                            a.Room.Id == appointmentModel.SelectedRoom &&
                            a.StartTime == appointmentModel.StartTime &&
                            a.EndTime == appointmentModel.EndTime);
            if (existingAppointment != null)
            {
                appointmentModel.Ressources = _context.Ressources.ToList();
                ViewBag.RoomList = _context
                    .Rooms
                    .OrderBy(r => r.Name)
                    .ToList();

                return View(appointmentModel);
            }


            //create variables
            var appointment = new Appointment
            {
                StartTime = appointmentModel.StartTime,
                EndTime = appointmentModel.EndTime,
                AppointmentRessources = new List<AppointmentRessource>()
            };

            var bookedTime = new BookedTime
            {
                StartTime = appointmentModel.StartTime,
                EndTime = appointmentModel.EndTime,
                RessourcesBookedTimes = new List<RessourceBookedTime>()
            };

            var ressourceList = new List<Ressource>();

            var rbt = new RessourceBookedTime();

            appointmentModel.Ressources = new List<Ressource>();



            //get related Survey object from database and store it into the Appointment object
            appointment.Survey = _context
                .Surveys
                .Include(s => s.Creator)
                .FirstOrDefault(s => s.Id == appointmentModel.Survey.Id);

            //necessary for validation check
            appointmentModel.Survey = appointment.Survey;

            //get selected Room 
            appointment.Room = (Room)_context
                .Ressources
                .Include(r => r.RessourceBookedTimes)
                    .ThenInclude(s => s.Ressource)
                .Include(r => r.RessourceBookedTimes)
                    .ThenInclude(s => s.BookedTime)
                .SingleOrDefault(r => r.Id == appointmentModel.SelectedRoom);

            //necessary for validation check
            appointmentModel.Room = appointment.Room;

            //add selected ressources to the ressource list
            if (appointmentModel.SelectedRessource != null)
            {
                foreach (var item in appointmentModel.SelectedRessource)
                {
                    ressourceList.Add(
                        _context
                            .Ressources
                            .Include(r => r.RessourceBookedTimes)
                                .ThenInclude(s => s.Ressource)
                            .Include(r => r.RessourceBookedTimes)
                                .ThenInclude(s => s.BookedTime)
                            .SingleOrDefault(r => r.Id == item)
                    );
                }
            }



            rbt = new RessourceBookedTime
            {
                BookedTime = bookedTime,
                BookedTimeId = bookedTime.Id,

                Ressource = appointment.Room,
                RessourceId = appointment.Room.Id
            };


            //book all ressources
            appointment.Room.RessourceBookedTimes.Add(rbt);
            bookedTime.RessourcesBookedTimes.Add(rbt);
            _context.Add(rbt);


            if (appointmentModel.SelectedRessource != null)
            {
                foreach (var item in ressourceList)
                {
                    if (item.RessourceBookedTimes == null)
                    {
                        item.RessourceBookedTimes = new List<RessourceBookedTime>();
                    }

                    rbt = new RessourceBookedTime
                    {
                        BookedTime = bookedTime,
                        BookedTimeId = bookedTime.Id,

                        Ressource = item,
                        RessourceId = item.Id
                    };

                    item.RessourceBookedTimes.Add(rbt);
                    bookedTime.RessourcesBookedTimes.Add(rbt);
                    _context.Add(rbt);

                    //create relationship object for appointment and ressource
                    var appointmentRessource = new AppointmentRessource
                    {
                        Appointment = appointment,
                        AppointmentId = appointment.Id,
                        Ressource = item,
                        RessourceId = item.Id
                    };

                    if (item.AppointmentRessources == null)
                    {
                        item.AppointmentRessources = new List<AppointmentRessource>();
                    }
                    item.AppointmentRessources.Add(appointmentRessource);

                    if (appointment.AppointmentRessources == null)
                    {
                        appointment.AppointmentRessources = new List<AppointmentRessource>();
                    }
                    appointment.AppointmentRessources.Add(appointmentRessource);

                    //add relationship model to db
                    _context.Add(appointmentRessource);

                    //this line maybe can be deleted
                    appointmentModel.Ressources.Add(item);
                }
            }


            //survey is explicitly loaded. SelectedMember is a direct entity. This
            //entity will never be saved into database. Everytime it's null.
            //It's a requuired entity for the survey and is also checked during 
            //appointment validation. Without this line it will always be unvalidated!
            appointmentModel.Survey.SelectedMember = new Collection<string>();

            //After model binding and validation are complete, you may want to repeat parts
            //of it. For example, a user may have entered text in a field expecting an 
            //integer, or you may need to compute a value for a model's property.
            ModelState.Clear();
            TryValidateModel(appointmentModel);

            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                _context.Add(bookedTime);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Surveys", new { id = appointmentModel.Survey.Id });
            }
            else
            {
                appointmentModel.Ressources = _context.Ressources.ToList();
                ViewBag.RoomList = _context
                    .Rooms
                    .OrderBy(r => r.Name)
                    .ToList();
            }
            return View(appointmentModel);
        }



        /// <summary>
        /// GET: Appointments/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                .Include(a => a.AppointmentRessources)
                .SingleOrDefaultAsync(m => m.Id == id);

            

            if (appointment == null)
            {
                return NotFound();
            }

            //create view model 
            var model = new AppointmentViewModel
            {
                AppointmentId = appointment.Id,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                Room = appointment.Room,
                Survey = appointment.Survey,
                Ressources = new List<Ressource>(),
                AppointmentRessources = appointment.AppointmentRessources,

            };

            foreach (var ar in model.AppointmentRessources)
            {
                model.Ressources.Add( _context
                    .AppointmentRessources
                    .Include(a => a.Ressource)
                    .SingleOrDefault(a => a.Id == ar.Id)
                    .Ressource );
            }


            //create List of all rooms for dropdown menue to be selected
            ViewBag.RoomList = _context
                .Rooms
                .OrderBy(r => r.Name)
                .ToList();

            //create List of all ressources for dropdown menue to be selected
            ViewBag.RessourceList = _context
                .Ressources
                .OrderBy(r => r.Name)
                .ToList();

            return View(model);
        }



        /// <summary>
        /// POST: Appointments/Edit/5
        /// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        /// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="appointmentModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,StartTime,EndTime,SelectedRoom")] AppointmentViewModel appointmentModel)
        {

            if (id != appointmentModel.AppointmentId)
            {
                return NotFound();
            }

            //initialize necessary temp variables:
            var appointment = _context
                .Appointments
                .Include(a => a.Survey)
                    .ThenInclude(s => s.Creator)
                .Include(a => a.Survey)
                    .ThenInclude(s => s.Appointments)
                .Include(a => a.Room)
                    .ThenInclude(r => r.AppointmentRessources)
                .Include(a => a.Room)
                    .ThenInclude(r => r.RessourceBookedTimes)
                .Include(a => a.AppointmentRessources)
                    .ThenInclude(ar => ar.Appointment)
                .Include(a => a.AppointmentRessources)
                    .ThenInclude(ar => ar.Ressource)
                .SingleOrDefault(a => a.Id == appointmentModel.AppointmentId);

            var newStartTime = appointmentModel.StartTime;
            var newEndTime = appointmentModel.EndTime;
            var newSelectedRoom = appointmentModel.SelectedRoom;

            var oldRoom = new Room();
            var newRoom = _context
                .Rooms
                .Include(r => r.RessourceBookedTimes)
                .SingleOrDefault(r => r.Id == appointmentModel.SelectedRoom);

            var oldBookedTime = new BookedTime();
            var newBookedTime = new BookedTime();

            var oldRessourceBookedTime = new RessourceBookedTime();
            var newRessourceBookedTime = new RessourceBookedTime();


            //avoid model validation problem
            if (appointment.Survey.SelectedMember == null)
            {
                appointment.Survey.SelectedMember = new Collection<string>();
            }


            //test if Room or Time changed.
            if (appointment.Room != newRoom)
            {
                //room changed
                oldRessourceBookedTime = _context
                    .RessourceBookedTimes
                    .Include(r => r.BookedTime)
                    .Include(r => r.Ressource)
                    .SingleOrDefault(r => r.Ressource.Id == appointment.Room.Id &&
                                        r.BookedTime.StartTime == appointment.StartTime &&
                                        r.BookedTime.EndTime == appointment.EndTime);


                //get old room and old bookedtime
                oldBookedTime = oldRessourceBookedTime.BookedTime;
                oldRoom = (Room)oldRessourceBookedTime.Ressource;


                //remove connection between ressource and bookedtime in the objects itself
                oldRoom.RessourceBookedTimes.Remove(oldRessourceBookedTime);
                oldBookedTime.RessourcesBookedTimes.Remove(oldRessourceBookedTime);


                //set connection between new room and booked time
                newRessourceBookedTime.BookedTime = oldBookedTime;
                newRessourceBookedTime.BookedTimeId = oldBookedTime.Id;
                newRessourceBookedTime.Ressource = newRoom;
                newRessourceBookedTime.RessourceId = newRoom.Id;

                newRoom.RessourceBookedTimes.Add(newRessourceBookedTime);


                //set new Room in appointment
                appointment.Room = newRoom;

            }


            if (appointment.StartTime == newStartTime && appointment.EndTime == newEndTime)
            {
                //time did not change!! 
            }
            else
            {
                //time changed and must be updated in room and all ressources

                if (oldRessourceBookedTime == null)     //if not null (because room changed too) the variable
                {                                       //must not be set again to avoid it beeing null.
                    oldRessourceBookedTime = _context   //the reason for this behavior is acutally unclear.
                        .RessourceBookedTimes
                        .Include(r => r.BookedTime)
                        .Include(r => r.Ressource)
                        .SingleOrDefault(r => r.Ressource.Id == appointment.Room.Id &&
                                    r.BookedTime.StartTime == appointment.StartTime &&
                                    r.BookedTime.EndTime == appointment.EndTime);
    
                }
                


                //get old bookedtime
                oldBookedTime = oldRessourceBookedTime.BookedTime;


                //get all RessourceBookedTimes with the same BookedTime ID
                var rbtList = _context
                    .RessourceBookedTimes
                    .Include(r => r.BookedTime)
                        .ThenInclude(b => b.RessourcesBookedTimes)
                    .Include(r => r.Ressource)
                    .Where(r => r.BookedTimeId == oldBookedTime.Id);

                //create new bookedTime that will be referenced in all ressources by the RessourceBookedTime object 
                newBookedTime = new BookedTime()
                {
                    StartTime = newStartTime,
                    EndTime = newEndTime,
                    RessourcesBookedTimes = new List<RessourceBookedTime>()
                };


                //update all RessourceBookedTime elements with the new BookedTime
                foreach (var rbt in rbtList)
                {
                    rbt.BookedTime = newBookedTime;
                    rbt.BookedTimeId = newBookedTime.Id;
                    newBookedTime.RessourcesBookedTimes.Add(rbt);

                    //update the rbt object
                    _context.Update(rbt);
                }
                _context.Update(newBookedTime);


                //remove old bookedtime from db. No longer used / refereced
                _context.BookedTimes.Remove(oldBookedTime);

                appointment.StartTime = newStartTime;
                appointment.EndTime = newEndTime;

            }


            //create List of all rooms for dropdown menue to be selected
            ViewBag.RoomList = _context
                .Rooms
                .OrderBy(r => r.Name)
                .ToList();

            //create List of all ressources for dropdown menue to be selected
            ViewBag.RessourceList = _context
                .Ressources
                .OrderBy(r => r.Name)
                .ToList();


            //set view model data
            appointmentModel = new AppointmentViewModel
            {
                AppointmentId = appointment.Id,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                Room = appointment.Room,
                Survey = appointment.Survey,
                Ressources = new List<Ressource>(),
                AppointmentRessources = appointment.AppointmentRessources,

            };

            foreach (var ar in appointmentModel.AppointmentRessources)
            {
                appointmentModel.Ressources.Add(_context
                    .AppointmentRessources
                    .Include(a => a.Ressource)
                    .SingleOrDefault(a => a.Id == ar.Id)
                    .Ressource);
            }


            ModelState.Clear();
            TryValidateModel(appointmentModel);

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
            return View(appointmentModel);
        }





        /// <summary>
        /// POST: Surveys/AddRessource
        /// adds an additional User to the existing member list of the survey
        /// </summary>
        /// <param name="appointment"></param>
        /// <param name="selectedRessource"></param>

        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> AddRessource([Bind("AppointmentId,Ressource")] AppointmentViewModel appointmentModel, int selectedRessource)
        {
            //load appointment from DB
            var appointment = _context
                .Appointments
                .Include(a => a.Survey)
                    .ThenInclude(s => s.Creator)
                .Include(a => a.Survey)
                    .ThenInclude(s => s.Appointments)
                .Include(a => a.Room)
                    .ThenInclude(r => r.AppointmentRessources)
                .Include(a => a.Room)
                    .ThenInclude(r => r.RessourceBookedTimes)
                .Include(a => a.AppointmentRessources)
                    .ThenInclude(ar => ar.Appointment)
                .Include(a => a.AppointmentRessources)
                    .ThenInclude(ar => ar.Ressource)
                .SingleOrDefault(a => a.Id == appointmentModel.AppointmentId);


            //load selected ressource object form DB
            var newRessource = _context
                    .Ressources
                    .Include(r => r.RessourceBookedTimes)
                    .SingleOrDefault(r => r.Id == selectedRessource);

            //check if ressource is already member of the appointment
            if (appointment.Room.Id == selectedRessource)
            {
                return RedirectToAction("Edit", appointment);
            }

            foreach (var ar in appointment.AppointmentRessources)
            {
                if (ar.Ressource.Id == selectedRessource)
                {
                    return RedirectToAction("Edit", appointment);
                }
            }



            //get bookedtime that belongs to this appointment
            var bookedTime = (_context
                    .RessourceBookedTimes
                    .Include(r => r.BookedTime)
                    .Include(r => r.Ressource)
                    .SingleOrDefault(r => r.Ressource.Id == appointment.Room.Id &&
                                        r.BookedTime.StartTime == appointment.StartTime &&
                                        r.BookedTime.EndTime == appointment.EndTime)
                ).BookedTime;



            //if ressource has never been booked befor, create new RBT list
            if (newRessource.RessourceBookedTimes == null)
            {
                newRessource.RessourceBookedTimes = new List<RessourceBookedTime>();
            }

            //add ressource to appointment and book it
            var newRBT = new RessourceBookedTime
            {
                BookedTime = bookedTime,
                BookedTimeId = bookedTime.Id,
                Ressource = newRessource,
                RessourceId = newRessource.Id
            };

            bookedTime.RessourcesBookedTimes.Add(newRBT);
            newRessource.RessourceBookedTimes.Add(newRBT);


            //add relationship between ressource and appointment
            var appRess = new AppointmentRessource
            {
                Appointment = appointment,
                AppointmentId = appointment.Id,
                Ressource = newRessource,
                RessourceId = newRessource.Id
            };

            if (newRessource.AppointmentRessources == null)
            {
                newRessource.AppointmentRessources = new List<AppointmentRessource>();
            }
            newRessource.AppointmentRessources.Add(appRess);
            appointment.AppointmentRessources.Add(appRess);


            //write changes into DB
            _context.Add(newRBT);
            _context.Add(appRess);
            _context.Update(bookedTime);
            _context.Update(newRessource);
            _context.Update(appointment);
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


            //set view model data
            appointmentModel = new AppointmentViewModel
            {
                AppointmentId = appointment.Id,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                Room = appointment.Room,
                Survey = appointment.Survey,
                Ressources = new List<Ressource>(),
                AppointmentRessources = appointment.AppointmentRessources,

            };

            foreach (var ar in appointmentModel.AppointmentRessources)
            {
                appointmentModel.Ressources.Add(_context
                    .AppointmentRessources
                    .Include(a => a.Ressource)
                    .SingleOrDefault(a => a.Id == ar.Id)
                    .Ressource);
            }

            return RedirectToAction("Edit", appointmentModel);
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="ressourceId"></param>
        ///// <param name="appointmentId"></param>
        ///// <returns></returns>
        //[Authorize(Roles = "Administrator,User")]
        //[Route("Appointments/RemoveRessource/{ressourceId},{appointmentId}")] //attribute routing for specific url parameters
        //public async Task<IActionResult> RemoveRessource(int ressourceId, int appointmentId)
        //{

        //    //load appointment from DB
        //    var appointment = _context
        //        .Appointments
        //        .Include(a => a.Survey)
        //            .ThenInclude(s => s.Creator)
        //        .Include(a => a.Survey)
        //            .ThenInclude(s => s.Members)
        //        .Include(a => a.Room)
        //            .ThenInclude(r => r.RessourceBookedTimes)
        //        .Include(a => a.Ressources)
        //            .ThenInclude(res => res.RessourceBookedTimes)
        //        .SingleOrDefault(a => a.Id == appointmentId);

        //    //load ressource object for delete form DB
        //    var deletedRessource = _context
        //            .Ressources
        //            .Include(r => r.RessourceBookedTimes)
        //            .SingleOrDefault(r => r.Id == ressourceId);

        //    //load bookedtime that belongs to this appointment from DB
        //    var bookedTime = (_context
        //            .RessourceBookedTimes
        //            .Include(r => r.BookedTime)
        //            .Include(r => r.Ressource)
        //            .SingleOrDefault(r => r.Ressource.Id == appointment.Room.Id &&
        //                                r.BookedTime.StartTime == appointment.StartTime &&
        //                                r.BookedTime.EndTime == appointment.EndTime)
        //        ).BookedTime;


        //    //load RessourceBookTime object from DB
        //    var deletedRBT = _context
        //        .RessourceBookedTimes
        //        .Include(r => r.BookedTime)
        //        .Include(r => r.Ressource)
        //        .SingleOrDefault(r => r.Ressource == deletedRessource && r.BookedTime == bookedTime);


        //    //remove ressource from appointment
        //    bookedTime.RessourcesBookedTimes.Remove(deletedRBT);
        //    deletedRessource.RessourceBookedTimes.Remove(deletedRBT);
        //    appointment.Ressources.Remove(deletedRessource);



        //    _context.Update(deletedRessource);
        //    _context.Update(bookedTime);
        //    _context.Update(appointment);
        //    _context.Remove(deletedRBT);
        //    await _context.SaveChangesAsync();


        //    //create room list for view to be passed
        //    ViewBag.RoomList = _context
        //        .Rooms
        //        .OrderBy(r => r.Name)
        //        .ToList();

        //    return RedirectToAction("Edit", appointment);
        //}





        ///// <summary>
        ///// GET: Appointments/Delete/5
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[Authorize(Roles = "Administrator,User")]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var appointment = await _context
        //        .Appointments
        //        .Include(a => a.Room)
        //        .Include(a => a.Ressources)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (appointment == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(appointment);
        //}



        ///// <summary>
        ///// POST: Appointments/Delete/5
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administrator,User")]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var appointment = await _context
        //        .Appointments
        //        .Include(a => a.Ressources)
        //            .ThenInclude(r => r.RessourceBookedTimes)
        //        .Include(a => a.Room)
        //            .ThenInclude(r => r.RessourceBookedTimes)
        //        .SingleOrDefaultAsync(m => m.Id == id);


        //    //load bookedtime that belongs to this appointment from DB
        //    var bookedTime = (_context
        //            .RessourceBookedTimes
        //            .Include(r => r.BookedTime)
        //            .Include(r => r.Ressource)
        //            .SingleOrDefault(r => r.Ressource.Id == appointment.Room.Id &&
        //                                r.BookedTime.StartTime == appointment.StartTime &&
        //                                r.BookedTime.EndTime == appointment.EndTime)
        //        ).BookedTime;

        //    var rbtList = _context
        //        .RessourceBookedTimes
        //        .Include(r => r.BookedTime)
        //        .Include(r => r.Ressource)
        //        .Where(r => r.BookedTimeId == bookedTime.Id);


        //    //delete all RBT and BookedTime for the appointment
        //    foreach (var rbt in rbtList.ToList())
        //    {

        //        var ressource = _context
        //            .Ressources
        //            .Include(r => r.RessourceBookedTimes)
        //            .SingleOrDefault(r => r.Id == rbt.RessourceId);

        //        ressource.RessourceBookedTimes.Remove(rbt);

        //        _context.Update(ressource);
        //        _context.Remove(rbt);
        //    }

        //    _context.Remove(bookedTime);


        //    _context.Appointments.Remove(appointment);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}


        [Authorize(Roles = "Administrator,User")]
        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
