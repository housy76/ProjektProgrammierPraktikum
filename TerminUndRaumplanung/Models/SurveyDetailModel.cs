using System.Collections.Generic;
using AppData.Models;

namespace TerminUndRaumplanung.Models
{
    public class SurveyDetailModel
    {
        public int SurveyId { get; set; }
        public string Subject { get; set; }
        public ApplicationUser Creator { get; set; }
        public IEnumerable<ApplicationUser> Members { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; }
    }
}