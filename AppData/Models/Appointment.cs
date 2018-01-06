using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppData.Models
{
    public class Appointment
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Beginn")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]

        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "Ende")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndTime { get; set; }

        [Display(Name = "Ort")]
        [Required]
        public Room Room { get; set; }

        //necessary for the dropdownlists to store the selected
        //value and send it back to the controller.
        public int SelectedRoom { get; set; }


        [Display(Name = "Weitere Ressourcen")]
        public ICollection<Ressource> Ressources { get; set; }

        //necessary for the dropdownlists to store the selected 
        //value and send it back to the controller.
        public int SelectedRessource { get; set; }


        [Required]
        public Survey Survey { get; set; }
        //beim Zugriff auf die Daten der Survey (z.B. ID oder Creator) muss
        //dies über 
        //Survey.Id    oder     Survey.Creator
        //erfolgen

    }
}