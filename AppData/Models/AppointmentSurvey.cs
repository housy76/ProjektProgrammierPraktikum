using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public ApplicationUser Creator { get; set; }

        [Display(Name = "Teilnehmer")]
        public ICollection<ApplicationUser> Members { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; }
    }
}
