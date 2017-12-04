using System;
using System.Collections.Generic;
using System.Text;

namespace AppData.Models
{
    public class BookedTime
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}