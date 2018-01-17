using AppData.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppData
{
    /// <summary>
    /// Database Context that creates necessary tables
    /// </summary>
    public class AppointmentContext : IdentityDbContext<ApplicationUser>
    {
        public AppointmentContext(DbContextOptions options) : base(options) { }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Ressource> Ressources { get; set; }
        public DbSet<Beamer> Beamers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<BookedTime> BookedTimes { get; set; }
        public DbSet<RessourceBookedTime> RessourceBookedTimes { get; set; }
        public DbSet<AppointmentRessource> AppointmentRessources { get; set; }

    }
}
