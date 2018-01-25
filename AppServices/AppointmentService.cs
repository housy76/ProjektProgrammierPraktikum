using AppData;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

// Erstellt durch Heribert Stempfhuber
namespace AppServices
{
    /// <summary>
    /// Service implementing Interface
    /// </summary>
    public class AppointmentService : IAppointment
    {
        public AppointmentContext _context;

        public AppointmentService(AppointmentContext context)
        {
            _context = context;
        }

        public void Add(Appointment newAppointment, int surveyId)
        {
            _context.Add(newAppointment);
            _context.SaveChanges();
        }

        public IEnumerable<Appointment> GetAll()
        {
            return _context.Appointments
                .Include(a => a.Id)
                .Include(a => a.StartTime)
                .Include(a => a.EndTime)
                .Include(a => a.Ressources)
                .Include(a => a.Room)
                .Include(a => a.Survey);
        }

        public Appointment GetById(int id)
        {
            return GetAll()
                .FirstOrDefault(a => a.Id == id);
        }

        public DateTime GetEndTime(int id)
        {
            return
                GetAll()
                .FirstOrDefault(a => a.Id == id)
                .EndTime;
        }

        public ICollection<Ressource> GetRessources(int id)
        {
            return
                GetAll()
                .FirstOrDefault(a => a.Id == id)
                .Ressources;
        }

        //changed entity type from string to Room
        public Room GetRoom(int id)
        {
            return 
                GetAll()
                .FirstOrDefault(a => a.Id == id)
                .Room;
        }

        public DateTime GetStartTime(int id)
        {
            return
                GetAll()
                .FirstOrDefault(a => a.Id == id)
                .EndTime;
        }

        public Survey GetSurvey(int id)
        {
            return _context
                .Surveys
                .FirstOrDefault(s => s.Id == id);
        }
    }
}
