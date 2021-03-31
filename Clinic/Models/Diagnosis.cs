using System.Collections.Generic;

namespace Clinic.Models
{
    public class Diagnosis
    {
        public int DiagnosisId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        public List<Appointment> Appointments { get; set; }
    }
}