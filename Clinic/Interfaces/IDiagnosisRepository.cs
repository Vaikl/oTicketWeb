using Clinic.Models;
using System.Collections.Generic;

namespace Clinic.Interfaces
{
    public interface IDiagnosisRepository
    {
        IEnumerable<Diagnosis> Diagnoses { get; }

        IEnumerable<Diagnosis> GetAllDiagnoses();

        void SaveDiagnosis(Diagnosis diagnosis);

        Diagnosis DeleteDiagnosis(int diagnosisId);

        Diagnosis GetDiagnosisById(int diagnosisId);
    }
}