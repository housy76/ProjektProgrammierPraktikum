using System;
using System.Collections.Generic;
using System.Text;

namespace AppData.Models
{
    public class AppointmentRessource
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        
        public int RessourceId { get; set; }
        public Ressource Ressource { get; set; }

    }
}
