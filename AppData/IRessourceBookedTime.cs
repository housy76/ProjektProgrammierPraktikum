using AppData.Models;
using System.Collections.Generic;

// Erstellt durch Maximilian Freiberger
namespace AppData
{
    /// <summary>
    /// RessourceBookedTimes  Interface
    /// </summary>
    public interface IRessourceBookedTime
    {
        void Add(RessourceBookedTime newRessourceBookedTime);
        IEnumerable<RessourceBookedTime> GetAll();
        RessourceBookedTime GetById(int id);
        Ressource GetRessource(int id);
        BookedTime GetBookedTime(int id);
    }
}
