using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppData;
using AppData.Models;
using Microsoft.AspNetCore.Routing;
using TerminUndRaumplanung.Models;

namespace TerminUndRaumplanung.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly AppointmentContext _context;

        public AppointmentsController(AppointmentContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Appointments.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
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

        //// GET: Appointments/Create
        public IActionResult Create()
        {
            return View();
        }


        //Add a New Appointment to an existing Survey
        public IActionResult Add(int id)
        {
            var model = new Appointment
            {
                StartTime = System.DateTime.Now,
                EndTime = System.DateTime.Now,
                Survey = _context
                                .AppointmentSurveys
                                .SingleOrDefault(s => s.Id == id)
            };
            return View(model);
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
                    [Bind("Id,StartTime,EndTime,Room,Ressources")] Appointment appointment,
                    [Bind("surveyId")] int? surveyId
            )
        {
            appointment.Survey = _context.AppointmentSurveys.FirstOrDefault(s => s.Id == surveyId);

            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync(); 

                return RedirectToAction(
                    "Details",
                    "AppointmentSurveys",
                    //Man muss ein neues Objekt erzeugen, das wie die Variable der
                    //empfangenden Methode des Controllers heißt.
                    //Das Übergeben der id als direkte INT - Zahl funktioniert nicht.
                    new { id = surveyId });

                //return RedirectToAction(nameof(Index));
            }
            return View(appointment);
        }


        //public async Task<IActionResult> Create(
        //            [Bind("Id,StartTime,EndTime,Room,Ressources")] AppointmentAddModel addModel,
        //            [Bind("surveyId")] int? surveyId
        //    )
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(addModel);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(addModel);
        //}

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Survey)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            var sId = appointment.Survey.Id;

            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, 
                    //Binding für das appointment Objekt wird benötigt!!
                    //alle Entities des Bindings werden im Update-Befehl aktualisiert
                    
                    //Ohne Binding wird das Gesamte Objekt mit allen Entities aktualisiert.
                    //Dabei werden aber durch das Lazy Loading aus der Datenbank die
                    //zugehörigen Objekte, die als Entities vorhanden sind nicht automaitsch
                    //mitgeladen und sind somit null. Beim Update werden diese aber auch 
                    //mitaktualisiert und sind anschließend mit null in der Datenbank
                    [Bind("Id,StartTime,EndTime,Room,Ressources")] Appointment appointment, 
                    
                    //zusätzlicher Übergabeparameter für die SurveyId dieses Appointments
                    //wird benötigt, um nach dem Speichern wieder auf die Ansicht der 
                    //Survey zurück zu kehren, von der man gekommen ist.
                    [Bind("surveyId")] int? surveyId
            )
        {
            if (id != appointment.Id)
            {
                return NotFound();
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
                    "Details", 
                    "AppointmentSurveys",
                    
                    //Man muss ein neues Objekt erzeugen, das wie die Variable der
                    //empfangenden Methode des Controllers heißt.
                    //Das Übergeben der id als direkte INT - Zahl funktioniert nicht.
                    new { id = surveyId });
            }
            return View(appointment);
        }

        
        // GET: Appointments/Delete/5
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.SingleOrDefaultAsync(m => m.Id == id);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
