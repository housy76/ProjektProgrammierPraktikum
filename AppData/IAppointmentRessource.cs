using AppData.Models;
using System.Collections.Generic;

namespace AppData
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAppointmentRessource
    {
        void Add(AppointmentRessource newAppointmentRessource);
        IEnumerable<AppointmentRessource> GetAll();
        AppointmentRessource GetById(int id);
        Ressource GetRessource(int id);
        Appointment GetAppointment(int id);
    }
}
