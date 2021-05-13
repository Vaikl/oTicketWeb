using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models
{
    public class AppointmentDone
    {
        [Key]
        public int AppointmentId { get; set; }
        public string PatientId { get; set; }
        public string PatientFullName { get; set; }
        public string DoctorId { get; set; }
        public int DiagnosisId { get; set; }
        public string DiagnosName { get; set; }

        public decimal TotalSum { get; set; }

        public DateTime AppointmentPlaced { get; set; }

    }
}
