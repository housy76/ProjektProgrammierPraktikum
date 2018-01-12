using AppData;
using System.Collections.Generic;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AppServices
{
    /// <summary>
    /// Service implementing Interface
    /// </summary>
    public class RessourceService : IRessource
    {
        AppointmentContext _context;

        public RessourceService(AppointmentContext context)
        {
            _context = context;
        }
        public void Add(Ressource newRessource)
        {
            _context.Add(newRessource);
            _context.SaveChanges();
        }

        public IEnumerable<Ressource> GetAll()
        {
            return _context
                .Ressources
                .Include(r => r.Id)
                .Include(r => r.Name)
                .Include(r => r.RessourceBookedTimes);
        }

        public ICollection<RessourceBookedTime> GetBookedTimes(int id)
        {
            return GetAll()
                .FirstOrDefault(r => r.Id == id)
                .RessourceBookedTimes;
        }

        public Ressource GetById(int id)
        {
            return GetAll()
                .FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Ressource> GetByName(string name)
        {
            return _context
                .Ressources
                .Include(r => r.Name.Contains(name));
        }

        public string GetName(int id)
        {
            return GetAll()
                .FirstOrDefault(r => r.Id == id)
                .Name;
        }
    }
}
