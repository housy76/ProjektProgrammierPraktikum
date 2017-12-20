using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AppData.Models
{
    public class Room : Ressource
    {

        [Required]
        public int NumberOfSeats { get; set; }

        [Required]
        public bool BeamerIsAvailable { get; set; }

        [Required]
        public bool SpeakerIsAvailable { get; set; }
    }
}
