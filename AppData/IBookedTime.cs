using AppData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppData
{
    public interface IBookedTime
    {
        void Add(BookedTime newBookedTime);
        IEnumerable<BookedTime> GetAll();
        BookedTime GetById(int id);
        DateTime GetStartTime(int id);
        DateTime GetEndTime(int id);
        Ressource GetRessource(int id);
    }
}
