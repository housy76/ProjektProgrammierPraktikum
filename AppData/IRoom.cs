using AppData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppData
{
    public interface IRoom
    {
        void Add(Room newRoom);
        IEnumerable<Room> GetAll();
        Room GetById(int id);
        IEnumerable<Room> GetByName(string name);
        string GetName(int id);
        int GetNumberOfSeats(int id);
        bool HasBeamer(int id);
        bool HasSpeaker(int id);

    }
}
