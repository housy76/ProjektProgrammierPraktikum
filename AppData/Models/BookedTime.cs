using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Erstellt durch Daniel Hornauer
namespace AppData.Models
{
    /// <summary>
    /// Data Model for BookedTime
    /// </summary>
    public class BookedTime
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Beginn")]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "Ende")]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndTime { get; set; }


        [Required]
        [Display(Name = "Ressource")]
        public List<RessourceBookedTime> RessourcesBookedTimes { get; set; }
    }
}