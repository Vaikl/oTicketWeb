using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clinic.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Введите название категории")]
        [Display(Name = "Название категории")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите описание категории")]
        [Display(Name = "Описание категории")]
        public string Description { get; set; }

        public List<Service> Services { get; set; }
    }
}