using Microsoft.EntityFrameworkCore;
using System;

namespace AppointmentToolData
{
    public class AppointmentToolContext : DbContext
    {
        //Konstruktor:
        public AppointmentToolContext(DbContextOptions options) : base(options) { }
        public DbSet<AppointmentSurvey> AppointmentSurveys { get; set; }

    }
}
