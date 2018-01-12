using AppData.Models;
using System.Collections.Generic;

namespace AppData
{
    /// <summary>
    /// Survey Interface
    /// </summary>
    public interface ISurvey
    {
        IEnumerable<Survey> GetAll();
        Survey GetById(int id);
        string GetSubject(int id);
        ApplicationUser GetCreator(int id);
        ICollection<ApplicationUser> GetMembers(int id);
        IEnumerable<Appointment> GetAppointments(int surveyId);
        
    }
}
