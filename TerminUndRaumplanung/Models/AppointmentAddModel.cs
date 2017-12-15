using System;
using AppData.Models;

namespace TerminUndRaumplanung.Models
{
    internal class AppointmentAddModel
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Room Room { get; set; }
        public string Ressources { get; set; }
        public AppointmentSurvey Survey { get; set; }
    }
}