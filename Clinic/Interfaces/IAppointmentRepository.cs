using Clinic.Models;
using System.Collections.Generic;

namespace Clinic.Interfaces
{
    public interface IAppointmentRepository
    {
        void CreateAppointment(Appointment appointment);

        public IEnumerable<Appointment> Appointments { get; }

        IEnumerable<Appointment> GetAllAppointments();

        void SaveAppointment(Appointment appointment);
    }
}