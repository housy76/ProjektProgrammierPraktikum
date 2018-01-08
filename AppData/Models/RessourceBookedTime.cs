﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AppData.Models
{
    public class RessourceBookedTime
    {
        public int Id { get; set; }
        public int BookedTimeId { get; set; }
        public BookedTime BookedTime { get; set; }

        public int RessourceId { get; set; }
        public Ressource Ressource { get; set; }
    }
}