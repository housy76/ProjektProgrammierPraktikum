using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Erstellt durch Maximilian Freiberger
namespace AppData.Models
{
    /// <summary>
    /// Data Model for Ressource
    /// </summary>
    public abstract class Ressource
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name / Bezeichnung")]
        public string Name { get; set; }
        public List<RessourceBookedTime> RessourceBookedTimes { get; set; }

    }
}
