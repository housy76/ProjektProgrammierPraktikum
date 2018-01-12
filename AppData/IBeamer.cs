using AppData.Models;
using System.Collections.Generic;

namespace AppData
{
    /// <summary>
    /// Beamer Interface
    /// </summary>
    public interface IBeamer
    {
        void Add(Beamer newBeamer);
        IEnumerable<Beamer> GetAll();
        Beamer GetById(int id);
        IEnumerable<Beamer> GetAvailableBeamers();
    }
}
