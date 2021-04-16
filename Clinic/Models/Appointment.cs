using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic.Models
{
    public class Appointment
    {
        
        public int AppointmentId { get; set; }

        public List<AppointmentLine> AppointmentLines { get; set; }

        public string PatientId { get; set; }
        public string PatientFullName { get; set; }
        public string DoctorId { get; set; }
        public int DiagnosisId { get; set; }
        public string DiagnosName { get; set; }

        [BindNever]
        [ScaffoldColumn(false)]
        [Column(TypeName = "decimal(20,2)")]
        public decimal TotalSum { get; set; }

        [BindNever]
        [ScaffoldColumn(false)]
        public DateTime AppointmentPlaced { get; set; }

        public Diagnosis Diagnosis { get; set; }
    }
}