using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppData;
using AppData.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using TerminUndRaumplanung.Models;

namespace TerminUndRaumplanung.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class SurveysController : Controller
    {
        private readonly AppointmentContext _context;
        private ISurvey _survey;
        private readonly UserManager<ApplicationUser> _userManager;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="survey"></param>
        /// <param name="userManager"></param>
        public SurveysController(AppointmentContext context, ISurvey survey, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _survey = survey;
            _userManager = userManager;
        }



        /// <summary>
        /// GET: Surveys
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> Index()
        {

            //var user = _context.ApplicationUsers.SingleOrDefault(
            //    u => u.Id == _userManager.GetUserId(HttpContext.User)
            //    );

            if (User.IsInRole("Administrator"))
            {
                var content = await _context
                    .Surveys
                    .Include(s => s.Creator)
                    .Include(s => s.Members)
                    .ToListAsync();
                return View(content);

            }
            else
            {
                var content = await _context
                    .Surveys
                    .Include(s => s.Creator)
                    .Include(s => s.Members)
                    .Where(s => s.Creator.Id == _userManager.GetUserId(HttpContext.User))
                    .ToListAsync();
                return View(content);

            }

        }



        /// <summary>
        /// GET: Surveys/Details/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> Details(int? id)
        {
            var survey = await _context.Surveys
                .Include(s => s.Creator)
                .Include(s => s.Members)
                .Include(s => s.Appointments)
                    .ThenInclude(a => a.Room)
                .Include(s => s.Appointments)
                    .ThenInclude(a => a.AppointmentRessources)
                        .ThenInclude(ar => ar.Ressource)
                .SingleOrDefaultAsync(m => m.Id == id);


            //create List of Appointment for displaying them in the survey detail view
            var appointmentViewModelList = new List<AppointmentViewModel>();

            if (survey.Appointments != null)
            {
                //create a new appointment view model object for each appointment
                //and store it in a list for handing it over to the view in the viewbag
                foreach (var app in survey.Appointments)
                {
                    var appointment = new AppointmentViewModel
                    {
                        AppointmentId = app.Id,
                        StartTime = app.StartTime,
                        EndTime = app.EndTime,
                        Room = app.Room,
                        Ressources = new List<Ressource>()
                    };

                    if (app.AppointmentRessources != null)
                    {
                        foreach (var ar in app.AppointmentRessources)
                        {
                            appointment.Ressources.Add(ar.Ressource);
                        }
                    }

                    appointmentViewModelList.Add(appointment);
                    
                }



                //foreach (var app in survey.Appointments)
                //{
                //    var salvm = new AppointmentViewModel
                //    {
                //        AppointmentId = app.Id,
                //        StartTime = app.StartTime,
                //        EndTime = app.EndTime,
                //        Room = app.Room,
                //        Ressources = new List<Ressource>()
                //    };

                //    if (app.AppointmentRessources != null)
                //    {
                //        foreach (var ar in app.AppointmentRessources)
                //        {
                //            salvm.Ressources.Add(ar.Ressource);
                //        }
                //    }

                //    appointmentList.Add(salvm);
                //}
            }

            ViewBag.AppointmentList = appointmentViewModelList;

            return View(survey);
        }



        /// <summary>
        /// GET: Surveys/Create
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator,User")]
        public IActionResult Create()
        {
            var model = new Survey
            {
                Members = _context
                    .ApplicationUsers
                    .ToList()
            };

            return View(model);
        }



        /// <summary>
        /// POST: Surveys/Create
        /// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        /// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> Create([Bind("Id,Subject,SelectedMember")] Survey survey)
        {
            //set current user as creator
            survey.Creator = _context
                    .ApplicationUsers
                    .FirstOrDefault(a => a.Id.Contains(_userManager.GetUserId(HttpContext.User)));

            //members must be initialised before setting values!
            survey.Members = new Collection<ApplicationUser>();

            foreach (var item in survey.SelectedMember)
            {
                survey.Members.Add(
                    _context
                        .ApplicationUsers
                        .SingleOrDefault(u => u.Id == item)
                );
            }

            if (survey.Members == null)
            {
                return View(survey);
            }

            ModelState.Clear();
            TryValidateModel(survey);

            if (ModelState.IsValid)
            {
                _context.Add(survey);
                await _context.SaveChangesAsync();
                //redirect to the detail view of this survey
                return RedirectToAction("Details", "Surveys", new { id = survey.Id });
            }
            return View(survey);
        }



        /// <summary>
        /// GET: Surveys/Edit/5
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

            var survey = await _context.Surveys
                .Include(m => m.Creator)
                .Include(m => m.Members)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (survey == null)
            {
                return NotFound();
            }

            ViewBag.MemberList = _context
                .ApplicationUsers
                .Where(u => u.Id != survey.Creator.Id)
                .ToList();

            return View(survey);
        }




        /// <summary>
        /// POST: Surveys/Edit/5
        /// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        /// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="survey"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Subject,Creator,SelectedMembers,Members")] Survey survey)
        {
            if (id != survey.Id)
            {
                return NotFound();
            }

            ModelState.Clear();
            TryValidateModel(survey);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(survey);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SurveyExists(survey.Id))
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
            return View(survey);
        }



        /// <summary>
        /// POST: Surveys/UpdateMemberList
        /// adds an additional User to the existing member list of the survey
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="selectedMember"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> UpdateMemberList(
            [Bind("Id,Subject")]Survey survey,
            string selectedMember)
        {


            survey = _context
                .Surveys
                .Include(s => s.Creator)
                .Include(s => s.Members)
                .Include(s => s.Appointments)
                .SingleOrDefault(s => s.Id == survey.Id);

            survey.Members.Add(
                _context
                    .ApplicationUsers
                    .SingleOrDefault(u => u.Id == selectedMember)
                );

            _context.Update(survey);
            await _context.SaveChangesAsync();


            return RedirectToAction("Edit", survey);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator,User")]
        [Route("Surveys/RemoveMember/{userId},{surveyId}")] //attribute routing for specific 
                                                            //url parameters
        public async Task<IActionResult> RemoveMember(string userId, int surveyId)
        {

            var survey = _context
                .Surveys
                .Include(s => s.Creator)
                .Include(s => s.Members)
                .Include(s => s.Appointments)
                .SingleOrDefault(s => s.Id == surveyId);


            survey.Members.Remove(
                _context
                    .ApplicationUsers
                    .SingleOrDefault(u => u.Id == userId)
                );

            _context.Update(survey);
            await _context.SaveChangesAsync();


            return RedirectToAction("Edit", survey);
        }



        /// <summary>
        /// GET: Surveys/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentSurvey = await _context
                .Surveys
                .Include(m => m.Creator)
                .Include(m => m.Members)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (appointmentSurvey == null)
            {
                return NotFound();
            }

            return View(appointmentSurvey);
        }



        /// <summary>
        /// POST: Surveys/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var survey = await _context
                .Surveys
                .Include(s => s.Appointments)
                .Include(s => s.Creator)
                .Include(s => s.Members)
                .SingleOrDefaultAsync(m => m.Id == id);


            //unbook all ressources that have been booked in the appointments 
            //that belong to this survey!
            var appointmentList = survey.Appointments;

            //test if survey has appointments
            //if (appointmentList != null)
            //{
            //    //delete all appointments and related bookedtimes and ResBoTimes
            //    foreach (var appointm in appointmentList)
            //    {
            //        var appointment = await _context
            //        .Appointments
            //        .Include(a => a.Ressources)
            //            .ThenInclude(r => r.RessourceBookedTimes)
            //        .Include(a => a.Room)
            //            .ThenInclude(r => r.RessourceBookedTimes)
            //        .SingleOrDefaultAsync(m => m.Id == appointm.Id);


            //        //load bookedtime that belongs to this appointment from DB
            //        var bookedTime = (_context
            //                .RessourceBookedTimes
            //                .Include(r => r.BookedTime)
            //                .Include(r => r.Ressource)
            //                .SingleOrDefault(r => r.Ressource.Id == appointment.Room.Id &&
            //                                    r.BookedTime.StartTime == appointment.StartTime &&
            //                                    r.BookedTime.EndTime == appointment.EndTime)
            //            ).BookedTime;

            //        var rbtList = _context
            //            .RessourceBookedTimes
            //            .Include(r => r.BookedTime)
            //            .Include(r => r.Ressource)
            //            .Where(r => r.BookedTimeId == bookedTime.Id);


            //        //delete all ResBoTimes and BookedTime for the appointment
            //        foreach (var rbt in rbtList.ToList())
            //        {

            //            var ressource = _context
            //                .Ressources
            //                .Include(r => r.RessourceBookedTimes)
            //                .SingleOrDefault(r => r.Id == rbt.RessourceId);

            //            ressource.RessourceBookedTimes.Remove(rbt);

            //            _context.Update(ressource);
            //            _context.Remove(rbt);
            //        }

            //        _context.Remove(bookedTime);
            //        _context.Appointments.Remove(appointment);
            //    }
            //}


            _context.Update(survey);
            _context.Surveys.Remove(survey);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator,User")]
        private bool SurveyExists(int id)
        {
            return _context.Surveys.Any(e => e.Id == id);
        }
    }
}
