using Clinic.Database;
using Clinic.Interfaces;
using Clinic.Models;
using System.Collections.Generic;
using System.Linq;

namespace Clinic.Repositories
{
    public class DiagnosisRepository : IDiagnosisRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DiagnosisRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IEnumerable<Diagnosis> Diagnoses => _applicationDbContext.Diagnoses.ToList();

        public IEnumerable<Diagnosis> GetAllDiagnoses()
        {
            return Diagnoses.OrderBy(p => p.Name).ToList();
        }

        public Diagnosis GetDiagnosisById(int diagnosisId) => _applicationDbContext.Diagnoses.FirstOrDefault(d => d.DiagnosisId == diagnosisId);

        public void SaveDiagnosis(Diagnosis diagnosis)
        {
            if (diagnosis != null && diagnosis.DiagnosisId == 0)
            {
                _applicationDbContext.Diagnoses.Add(diagnosis);
            }
            else
            {
                Diagnosis dbEntry = _applicationDbContext.Diagnoses.FirstOrDefault(d => d.DiagnosisId == diagnosis.DiagnosisId);

                if (diagnosis != null && dbEntry != null)
                {
                    dbEntry.Name = diagnosis.Name;
                    dbEntry.Category = diagnosis.Category;
                    dbEntry.Description = diagnosis.Description;
                }
            }

            _applicationDbContext.SaveChanges();
        }

        public Diagnosis DeleteDiagnosis(int diagnosisId)
        {
            Diagnosis dbEntry = _applicationDbContext.Diagnoses.FirstOrDefault(d => d.DiagnosisId == diagnosisId);

            Diagnosis diagnosisToDelete = _applicationDbContext.Diagnoses.Where(d => d.DiagnosisId == diagnosisId).SingleOrDefault();
            var appointmentsToDelete = _applicationDbContext.Appointments.Where(a => a.Diagnosis == diagnosisToDelete).AsEnumerable();

            foreach (var appointment in appointmentsToDelete)
            {
                _applicationDbContext.Appointments.Remove(appointment);
            }

            if (dbEntry != null)
            {
                _applicationDbContext.Diagnoses.Remove(dbEntry);
                _applicationDbContext.SaveChanges();
            }

            return dbEntry;
        }
    }
}