using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AppData.Models
{
    public class AppointmentSurvey
    {
        public int Id { get; set; }


        [Required]
        public string Subject { get; set; }


        [Required]
        public string Creator { get; set; }


        [Required]
        public string Members { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; }
    }
}
