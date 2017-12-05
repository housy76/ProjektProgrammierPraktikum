using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppData;
using AppData.Models;
using TerminUndRaumplanung.Models;

namespace TerminUndRaumplanung.Controllers
{
    public class AppointmentSurveysController : Controller
    {
        private readonly AppointmentContext _context;
        private IAppointmentSurvey _survey;

        public AppointmentSurveysController(AppointmentContext context, IAppointmentSurvey survey)
        {
            _context = context;
            _survey = survey;
        }

        // GET: AppointmentSurveys
        public async Task<IActionResult> Index()
        {
            return View(await _context.AppointmentSurveys.ToListAsync());
        }

        // GET: AppointmentSurveys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var survey = await _context.AppointmentSurveys
                .SingleOrDefaultAsync(m => m.Id == id);

            var model = new SurveyDetailModel
            {
                SurveyId = survey.Id,
                Subject = survey.Subject,
                Creator = survey.Creator,
                Members = survey.Members,
                Appointments = _survey.GetAppointments(survey.Id),

            };

            return View(model);
        }

        // GET: AppointmentSurveys/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AppointmentSurveys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Subject,Creator,Members")] AppointmentSurvey appointmentSurvey)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointmentSurvey);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appointmentSurvey);
        }

        // GET: AppointmentSurveys/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentSurvey = await _context.AppointmentSurveys.SingleOrDefaultAsync(m => m.Id == id);
            if (appointmentSurvey == null)
            {
                return NotFound();
            }
            return View(appointmentSurvey);
        }

        // POST: AppointmentSurveys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Subject,Creator,Members")] AppointmentSurvey appointmentSurvey)
        {
            if (id != appointmentSurvey.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointmentSurvey);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentSurveyExists(appointmentSurvey.Id))
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
            return View(appointmentSurvey);
        }

        // GET: AppointmentSurveys/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentSurvey = await _context.AppointmentSurveys
                .SingleOrDefaultAsync(m => m.Id == id);
            if (appointmentSurvey == null)
            {
                return NotFound();
            }

            return View(appointmentSurvey);
        }

        // POST: AppointmentSurveys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointmentSurvey = await _context.AppointmentSurveys.SingleOrDefaultAsync(m => m.Id == id);
            _context.AppointmentSurveys.Remove(appointmentSurvey);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentSurveyExists(int id)
        {
            return _context.AppointmentSurveys.Any(e => e.Id == id);
        }
    }
}
