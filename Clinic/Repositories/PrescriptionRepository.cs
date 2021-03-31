using Clinic.Database;
using Clinic.Interfaces;
using Clinic.Models;
using System.Collections.Generic;
using System.Linq;

namespace Clinic.Repositories
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public PrescriptionRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IEnumerable<Prescription> Prescriptions => _applicationDbContext.Prescriptions.ToList();

        public IEnumerable<Prescription> GetAllPrescriptions()
        {
            return Prescriptions.OrderBy(p => p.PrescriptionDate).ToList();
        }

        public Prescription GetPrescriptionById(int prescriptionId) => _applicationDbContext.Prescriptions.FirstOrDefault(d => d.PrescriptionId == prescriptionId);

        public void SavePrescription(Prescription prescription)
        {
            if (prescription != null && prescription.PrescriptionId == 0)
            {
                _applicationDbContext.Prescriptions.Add(prescription);
            }
            else
            {
                Prescription dbEntry = _applicationDbContext.Prescriptions.FirstOrDefault(d => d.PrescriptionId == prescription.PrescriptionId);

                if (prescription != null && dbEntry != null)
                {
                    dbEntry.PrescriptionDate = prescription.PrescriptionDate;
                    dbEntry.Description = prescription.Description;
                    dbEntry.Meds = prescription.Meds;
                    dbEntry.PatientName = prescription.PatientName;
                }
            }

            _applicationDbContext.SaveChanges();
        }
    }
}