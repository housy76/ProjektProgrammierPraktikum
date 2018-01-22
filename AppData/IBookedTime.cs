using AppData.Models;
using System;
using System.Collections.Generic;

// Erstellt durch Daniel Hornauer
namespace AppData
{
    /// <summary>
    /// BookedTime Interface
    /// </summary>
    public interface IBookedTime
    {
        void Add(BookedTime newBookedTime);
        IEnumerable<BookedTime> GetAll();
        BookedTime GetById(int id);
        DateTime GetStartTime(int id);
        DateTime GetEndTime(int id);
        //ICollection<Ressource> GetRessources(int id);
    }
}
