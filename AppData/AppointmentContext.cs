using AppData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppData
{
    public class AppointmentContext : DbContext
    {
        public AppointmentContext(DbContextOptions options) : base(options) { }
        public DbSet<AppointmentSurvey> AppointmentSurveys { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Ressource> Ressources { get; set; }
        public DbSet<Beamer> Beamers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<BookedTime> BookedTimes { get; set; }

    }
}
