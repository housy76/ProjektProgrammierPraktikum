using AppData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppData
{
    public interface IRessource
    {
        IEnumerable<IRessource> GetAll();
        Ressource GetById(int id);
        string GetName(int id);
        BookedTime GetBookedTime(int id);

    }
}
