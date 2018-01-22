using AppData;
using System.Collections.Generic;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

// Erstellt durch Maximilian Freiberger
namespace AppServices
{
    /// <summary>
    /// Service implementing Interface
    /// </summary>
    public class RoomService : IRoom
    {
        AppointmentContext _context;

        public RoomService(AppointmentContext context)
        {
            _context = context;
        }

        public void Add(Room newRoom)
        {
            _context.Add(newRoom);
            _context.SaveChanges();
        }

        public IEnumerable<Room> GetAll()
        {
            return _context
                .Rooms
                .Include(r => r.Id);
        }

        public Room GetById(int id)
        {
            return GetAll()
                .FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Room> GetByName(string name)
        {
            return _context
                .Rooms
                .Include(r => r.Name.Contains(name)); 
        }

        public string GetName(int id)
        {
            return GetAll()
                .FirstOrDefault(r => r.Id == id)
                .Name;
        }

        public int GetNumberOfSeats(int id)
        {
            return GetAll()
                .FirstOrDefault(r => r.Id == id)
                .NumberOfSeats;
        }

        public bool HasBeamer(int id)
        {
            return GetAll()
                .FirstOrDefault(r => r.Id == id)
                .BeamerIsAvailable;
        }

        public bool HasSpeaker(int id)
        {
            return GetAll()
                 .FirstOrDefault(r => r.Id == id)
                 .SpeakerIsAvailable;
        }
    }
}
