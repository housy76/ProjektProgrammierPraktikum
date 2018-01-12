using AppData;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppServices
{
    /// <summary>
    /// Service implementing Interface
    /// </summary>
    public class RessourceBookedTimeService : IRessourceBookedTime
    {
        AppointmentContext _context;

        public RessourceBookedTimeService(AppointmentContext context)
        {
            _context = context;
        }

        public void Add(RessourceBookedTime newRessourceBookedTime)
        {
            _context.Add(newRessourceBookedTime);
            _context.SaveChanges();
        }

        public IEnumerable<RessourceBookedTime> GetAll()
        {
            return _context
                .RessourceBookedTimes
                .Include(rbt => rbt.BookedTime)
                .Include(rbt => rbt.Ressource);
        }

        public BookedTime GetBookedTime(int id)
        {
            throw new NotImplementedException();
        }

        public RessourceBookedTime GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Ressource GetRessource(int id)
        {
            throw new NotImplementedException();
        }
    }
}
