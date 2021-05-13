using System;
using System.ComponentModel.DataAnnotations;

namespace Clinic.Models
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }
        public DateTime PrescriptionDate { get; set; }

        [Required(ErrorMessage = "Введите полное описание")]
        [Display(Name = "Полное описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Введите список препаратов")]
        [Display(Name = "Список препаратов")]
        public string Meds { get; set; }
        public string PatientName { get; set; }
    }
}