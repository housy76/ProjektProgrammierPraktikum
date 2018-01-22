using AppData;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

// Erstellt Daniel Hornauer
namespace AppServices
{
    /// <summary>
    /// Service implementing Interface
    /// </summary>
    public class BookedTimeService : IBookedTime
    {
        private AppointmentContext _context;

        public BookedTimeService(AppointmentContext context)
        {
            _context = context;
        }

        public void Add(BookedTime newBookedTime)
        {
            _context.Add(newBookedTime);
            _context.SaveChanges();
        }

        public IEnumerable<BookedTime> GetAll()
        {
            return _context
                .BookedTimes
                .Include(b => b.Id)
                .Include(b => b.StartTime)
                .Include(b => b.EndTime);
                //.Include(b => b.Ressource);
        }

        public BookedTime GetById(int id)
        {
            return GetAll()
                .FirstOrDefault(b => b.Id == id);
        }

        public DateTime GetEndTime(int id)
        {
            return GetAll()
                .SingleOrDefault(b => b.Id == id)
                .EndTime;
        }

        //public ICollection<Ressource> GetRessources(int id)
        //{
        //    return GetAll()
        //        .SingleOrDefault(b => b.Id == id)
        //        .Ressources;
        //}

        public DateTime GetStartTime(int id)
        {
            return GetAll()
                .SingleOrDefault(b => b.Id == id)
                .StartTime;
        }

        //public ICollection<Ressource> GetRessources(int id)
        //{
        //    return GetAll()
        //        .SingleOrDefault(b => b.Id == id)
        //        .Ressources;
        //}
    }
}
