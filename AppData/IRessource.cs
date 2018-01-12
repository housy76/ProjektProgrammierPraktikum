using AppData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppData
{
    /// <summary>
    /// Ressource Interface
    /// </summary>
    public interface IRessource
    {
        void Add(Ressource newRessource);
        IEnumerable<Ressource> GetAll();
        Ressource GetById(int id);
        IEnumerable<Ressource> GetByName(string name);
        string GetName(int id);
        ICollection<RessourceBookedTime> GetBookedTimes(int id);

    }
}
