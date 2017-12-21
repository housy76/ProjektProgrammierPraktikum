using AppData;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppServices
{
    public class AppointmentSurveyService : IAppointmentSurvey
    {
        public AppointmentContext _context;

        public AppointmentSurveyService(AppointmentContext context)
        {
            _context = context;
        }

        public void Add(AppointmentSurvey newSurvey)
        {
            _context.Add(newSurvey);
            _context.SaveChanges();
        }

        public IEnumerable<AppointmentSurvey> GetAll()
        {
            return _context.AppointmentSurveys
                            .Include(a => a.Subject);
        }

        public IEnumerable<Appointment> GetAppointments(int surveyId)
        {
            return _context.AppointmentSurveys
                            .Include(s => s.Appointments)
                            .FirstOrDefault(a => a.Id == surveyId)
                            .Appointments;
        }

        public AppointmentSurvey GetById(int id)
        {
            return _context.AppointmentSurveys
                            .Include(a => a.Subject)
                            .Include(a => a.Creator)
                            .FirstOrDefault(a => a.Id == id);
        }

        public string GetCreator(int id)
        {
            return
                            GetAll()
                            .FirstOrDefault(a => a.Id == id)
                            .Creator;
        }

        public string GetMembers(int id)
        {
            return
                GetAll()
                .FirstOrDefault(a => a.Id == id)
                .Members;
        }

        public string GetSubject(int id)
        {
            return
                           GetAll()
                           .FirstOrDefault(a => a.Id == id)
                           .Subject;
        }
    }
}
