using AppData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppData
{
    public interface IAppointment
    {
        IEnumerable<Appointment> GetAll();
        Appointment GetById(int id);
        string GetRoom(int id);
        string GetRessources(int id);
        DateTime GetStartTime(int id);
        DateTime GetEndTime(int id);
        AppointmentSurvey GetSurvey(int id);
    }

}