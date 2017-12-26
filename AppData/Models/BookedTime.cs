using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AppData.Models
{
    public class BookedTime
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Beginn")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "Ende")]
        public DateTime EndTime { get; set; }


        [Required]
        [Display(Name = "Ressource")]
        public Ressource Ressource { get; set; }
    }
}