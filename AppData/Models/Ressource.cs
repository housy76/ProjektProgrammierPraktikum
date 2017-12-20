using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AppData.Models
{
    public abstract class Ressource
    {
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }
        public IEnumerable<BookedTime> BookedTimes { get; set; }

    }
}
