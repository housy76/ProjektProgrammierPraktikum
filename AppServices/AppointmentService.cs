using AppData;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppServices
{
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
                .Include(a => a.StartTime);
        }

        public Appointment GetById(int id)
        {
            return _context.Appointments
                .Include(a => a.StartTime)
                .Include(a => a.EndTime)
                .Include(a => a.Room)
                .Include(a => a.Ressources)
                .FirstOrDefault(a => a.Id == id);
        }

        public DateTime GetEndTime(int id)
        {
            return
                GetAll()
                .FirstOrDefault(a => a.Id == id)
                .EndTime;
        }

        public string GetRessources(int id)
        {
            return
                GetAll()
                .FirstOrDefault(a => a.Id == id)
                .Ressources;
        }

        //Simon
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
                .StartTime;
        }

        public AppointmentSurvey GetSurvey(int id)
        {
            //return
            //    GetAll()
            //    .FirstOrDefault(a => a.Id == id)
            //    .SurveyId;

            return _context
                .AppointmentSurveys
                .FirstOrDefault(s => s.Id == id);
        }
    }
}
