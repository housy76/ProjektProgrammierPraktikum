using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AppData.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        //Simon
        //changed entity type from string to Room
        public Room Room { get; set; }
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