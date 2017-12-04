using System;
using System.Collections.Generic;
using System.Text;

namespace AppData.Models
{
    public class Room : Ressource
    {
        public int NumberOfSeats { get; set; }
        public bool BeamerIsAvailable { get; set; }
        public bool SpeakerIsAvailable { get; set; }
    }
}
