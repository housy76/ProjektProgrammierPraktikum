using AppData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppData
{
    public interface IAppointmentSurvey
    {
        IEnumerable<AppointmentSurvey> GetAll();
        AppointmentSurvey GetById(int id);
        string GetSubject(int id);
        ApplicationUser GetCreator(int id);
        ICollection<ApplicationUser> GetMembers(int id);
        IEnumerable<Appointment> GetAppointments(int surveyId);
    }
}
