using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Erstellt durch Marco Geisthoff
namespace AppData.Models
{
    /// <summary>
    /// Data Model for Survey
    /// </summary>
    public class Survey
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Betreff erforderlich!")]
        [Display(Name = "Betreff")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Ersteller")]
        public ApplicationUser Creator { get; set; }

        [Display(Name = "Teilnehmer")]
        public ICollection<ApplicationUser> Members { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Mindestens ein Teilnehmer erforderlich!")]
        public IEnumerable<string> SelectedMember { get; set; }

        [Display(Name = "Termine")]
        public IEnumerable<Appointment> Appointments { get; set; }
    }
}
