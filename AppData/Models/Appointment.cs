using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppData.Models
{
    public class Appointment
    {

        public int Id { get; set; }

        [Display(Name = "Beginn")]
        [Required]
        public DateTime StartTime { get; set; }

        [Display(Name = "Ende")]
        [Required]
        public DateTime EndTime { get; set; }

        [Display(Name = "Ort")]
        [Required]
        public Room Room { get; set; }

        [Display(Name = "Weitere Ressourcen")]
        public IEnumerable<Ressource> Ressources { get; set; }

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