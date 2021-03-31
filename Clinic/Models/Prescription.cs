using System;

namespace Clinic.Models
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public string Description { get; set; }
        public string Meds { get; set; }
        public string PatientName { get; set; }
    }
}