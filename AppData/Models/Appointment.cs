using System;
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

        [Display(Name = "Ressourcen")]
        public string Ressources { get; set; }

        //Entity die die Referenz zum entsprechenden AppointmentSurvey Objekt
        //darstellt wurde in Survey umbenannt. Es handelt sich hier um ein 
        //gesamtes Objekt und nicht nur um einen INT, der die ID der Survey 
        //enthält!!!!

        [Required]
        public AppointmentSurvey Survey { get; set; }
        //beim Zugriff auf die Daten der Survey (z.B. ID oder Creator) muss
        //dies über 
        //Survey.Id    oder     Survey.Creator
        //erfolgen

    }
}