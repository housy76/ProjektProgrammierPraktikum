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
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "Ende")]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndTime { get; set; }


        [Required]
        [Display(Name = "Ressource")]
        public List<RessourceBookedTime> RessourcesBookedTimes { get; set; }
    }
}