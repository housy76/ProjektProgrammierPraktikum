using AppData;
using System;
using System.Collections.Generic;
using System.Text;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AppServices
{
    /// <summary>
    /// Service implementing Interface
    /// </summary>
    public class BeamerService : IBeamer
    {
        AppointmentContext _context;

        public BeamerService(AppointmentContext context)
        {
            _context = context;
        }

        public void Add(Beamer newBeamer)
        {
            _context.Add(newBeamer);
            _context.SaveChanges();
        }

        public IEnumerable<Beamer> GetAll()
        {
            return _context
                .Beamers
                .Include(b => b.Id);
        }

        public IEnumerable<Beamer> GetAvailableBeamers()
        {
            return _context
                .Beamers
                //.Include(b => b.IsAvailable == true)
                .Include(b => b.Name);
        }

        public Beamer GetById(int id)
        {
            return GetAll()
                .FirstOrDefault(b => b.Id == id);
        }
    }
}
