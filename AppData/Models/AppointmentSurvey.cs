using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppData.Models
{
    public class AppointmentSurvey
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Betreff")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Ersteller")]
        public string Creator { get; set; }

        [Required]
        [Display(Name = "Teilnehmer")]
        public string Members { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; }
    }
}
