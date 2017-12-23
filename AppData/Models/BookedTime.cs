using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AppData.Models
{
    public class BookedTime
    {
        public int Id { get; set; }

        [Display(Name = "Beginn")]
        public DateTime StartTime { get; set; }

        [Display(Name = "Ende")]
        public DateTime EndTime { get; set; }
    }
}