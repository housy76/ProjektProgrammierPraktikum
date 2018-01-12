using AppData;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AppServices
{
    /// <summary>
    /// Service implementing Interface
    /// </summary>
    public class SurveyService : ISurvey
    {
        public AppointmentContext _context;

        public SurveyService(AppointmentContext context)
        {
            _context = context;
        }

        public void Add(Survey newSurvey)
        {
            _context.Add(newSurvey);
            _context.SaveChanges();
        }

        public IEnumerable<Survey> GetAll()
        {
            return _context.Surveys
                .Include(a => a.Subject);
        }

        public IEnumerable<Appointment> GetAppointments(int surveyId)
        {
            return _context.Surveys
                .Include(s => s.Appointments)
                .FirstOrDefault(a => a.Id == surveyId)
                .Appointments;
        }

        public Survey GetById(int id)
        {
            return _context.Surveys
                .Include(a => a.Subject)
                .Include(a => a.Creator)
                .FirstOrDefault(a => a.Id == id);
        }

        public ApplicationUser GetCreator(int id)
        {
            return
                GetAll()
                .FirstOrDefault(a => a.Id == id)
                .Creator;
        }

        public ICollection<ApplicationUser> GetMembers(int id)
        {
            return
                GetAll()
                .FirstOrDefault(s => s.Id == id)
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
