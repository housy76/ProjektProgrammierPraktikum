using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppData;
using AppData.Models;
using TerminUndRaumplanung.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Collections.ObjectModel;

namespace TerminUndRaumplanung.Controllers
{
    public class SurveysController : Controller
    {
        private readonly AppointmentContext _context;
        private ISurvey _survey;
        private readonly UserManager<ApplicationUser> _userManager;

        public SurveysController(AppointmentContext context, ISurvey survey, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _survey = survey;
            _userManager = userManager;
        }

        // GET: Surveys
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


        // GET: Surveys/Details/5
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> Details(int? id)
        {
            var survey = await _context.Surveys
                .Include(a => a.Creator)
                .Include(a => a.Members)
                .SingleOrDefaultAsync(m => m.Id == id);

            //Simon
            var model = new SurveyDetailModel
            {
                SurveyId = survey.Id,
                Subject = survey.Subject,
                Creator = survey.Creator,
                Members = survey.Members,
                Appointments = _context
                                    .Appointments
                                    .Include(a => a.Room)
                                    .Where(a => a.Survey.Id == survey.Id)
            };

            return View(model);
        }

        // GET: Surveys/Create
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

        // POST: Surveys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> Create([Bind("Id,Subject,Members,SelectedMember")] Survey survey)
        {
            //set current user as creator
            survey.Creator = _context
                    .ApplicationUsers
                    .FirstOrDefault(a => a.Id.Contains(_userManager.GetUserId(HttpContext.User)));

            //members must not be null!!!
            survey.Members = new Collection<ApplicationUser>
            {
                _context
                .ApplicationUsers
                .SingleOrDefault(u => u.Id == survey.SelectedMember)
            };
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

        // GET: Surveys/Edit/5
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

        
        // POST: Surveys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    if (!AppointmentSurveyExists(survey.Id))
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


        //POST: Surveys/UpdateMemberList
        //adds an additional User to the existing member list of the survey
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


        [Authorize(Roles = "Administrator,User")]
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


        // GET: Surveys/Delete/5
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentSurvey = await _context.Surveys
                .Include(m => m.Creator)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (appointmentSurvey == null)
            {
                return NotFound();
            }

            return View(appointmentSurvey);
        }

        // POST: Surveys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointmentSurvey = await _context.Surveys.SingleOrDefaultAsync(m => m.Id == id);
            _context.Surveys.Remove(appointmentSurvey);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator,User")]
        private bool AppointmentSurveyExists(int id)
        {
            return _context.Surveys.Any(e => e.Id == id);
        }
    }
}
