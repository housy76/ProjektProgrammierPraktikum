using AppData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppData
{
    public interface IRessourceBookedTime
    {
        void Add(RessourceBookedTime newRessourceBookedTime);
        IEnumerable<RessourceBookedTime> GetAll();
        RessourceBookedTime GetById(int id);
        Ressource GetRessource(int id);
        BookedTime GetBookedTime(int id);
    }
}
