using Clinic.Models;
using System.Collections.Generic;

namespace Clinic.Interfaces
{
    public interface IPrescriptionRepository
    {
        IEnumerable<Prescription> Prescriptions { get; }

        IEnumerable<Prescription> GetAllPrescriptions();

        void SavePrescription(Prescription prescription);

        Prescription GetPrescriptionById(int prescriptionId);
    }
}