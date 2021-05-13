using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clinic.Models
{
    public class Diagnosis
    {
        public int DiagnosisId { get; set; }

        [Required(ErrorMessage = "Введите название диагноза")]
        [Display(Name = "Название диагноза")]
        public string Name { get; set; }
        public string Category { get; set; }

        [Required(ErrorMessage = "Введите описание диагноза")]
        [Display(Name = "Описание диагноза")]
        public string Description { get; set; }

        public List<Appointment> Appointments { get; set; }
    }
}