using System;
using System.Collections.Generic;
using System.Text;

namespace AppData.Models
{
    public class AppointmentSurvey
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Creator { get; set; }
        public string Members { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; }
    }
}
