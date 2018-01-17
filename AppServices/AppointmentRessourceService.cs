using AppData;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppServices
{
    public class AppointmentRessourceService : IAppointmentRessource
    {
        AppointmentContext _context;

        public AppointmentRessourceService(AppointmentContext context)
        {
            _context = context;
        }

        public void Add(AppointmentRessource newAppointmentRessource)
        {
            _context.Add(newAppointmentRessource);
            _context.SaveChanges();
        }

        public IEnumerable<AppointmentRessource> GetAll()
        {
            return _context
                .AppointmentRessources
                .Include(ar => ar.Appointment)
                .Include(ar => ar.Ressource);
        }

        public Appointment GetAppointment(int id)
        {
            return GetAll()
                .SingleOrDefault(ar => ar.Id == id)
                .Appointment;
        }

        public AppointmentRessource GetById(int id)
        {
            return GetAll()
                .SingleOrDefault(ar => ar.Id == id);
        }

        public Ressource GetRessource(int id)
        {
            return GetAll()
                .SingleOrDefault(ar => ar.Id == id)
                .Ressource;
        }
    }
}
