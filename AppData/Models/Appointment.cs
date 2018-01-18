using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppData.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Beginn")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "Ende")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndTime { get; set; }

        [Required]
        [Display(Name = "Ort")]
        public Room Room { get; set; }

        //necessary for the dropdownlists to store the selected
        //value and send it back to the controller.
        [Required(ErrorMessage = "Ort ist erforderlich!")]
        public int SelectedRoom { get; set; }



        //entity for many-to-many connection
        [Display(Name = "Weitere Ressourcen")]
        public List<AppointmentRessource> AppointmentRessources { get; set; }


        [NotMapped]
        public IEnumerable<int> SelectedRessource { get; set; }


        [Required]
        public Survey Survey { get; set; }
    }
}