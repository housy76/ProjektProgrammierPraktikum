using System;
using System.Collections.Generic;
using System.Text;

namespace AppData.Models
{
    public abstract class Ressource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public BookedTime BookedTimes { get; set; }

    }
}
