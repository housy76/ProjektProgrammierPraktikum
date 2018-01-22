using AppData.Models;
using System;
using System.Collections.Generic;

// Erstellt durch Herbiert Stempfhuber
namespace AppData
{
    /// <summary>
    /// Appointment Interface
    /// </summary>
    public interface IAppointment
    {
        IEnumerable<Appointment> GetAll();
        Appointment GetById(int id);

        //Simon
        //changed entity type from string to Room
        Room GetRoom(int id);
        ICollection<Ressource> GetRessources(int id);
        DateTime GetStartTime(int id);
        DateTime GetEndTime(int id);
        Survey GetSurvey(int id);
    }

}