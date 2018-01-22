using AppData.Models;
using System.Collections.Generic;

// Erstellt durch Maximilian Freiberger
namespace AppData
{
    /// <summary>
    /// Room Interface
    /// </summary>
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
