using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic.Models
{
    public class Service
    {
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Введите Имя")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите краткое описание")]
        [Display(Name = "Краткое описание")]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "Введите полное описание")]
        [Display(Name = "Полное описание")]
        public string LongDescription { get; set; }

        [Required(ErrorMessage = "Введите цену")]
        [Display(Name = "Цена")]
        [Column(TypeName = "decimal(20,2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Введите ID врача")]
        [Display(Name = "ID Врача")]
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }

        [Required(ErrorMessage = "Введите ссылку на картинку")]
        [Display(Name = "Ссылка на картинку")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Введите ссылку на превью картинки")]
        [Display(Name = "Превью картинки")]
        public string ImageThumbnailUrl { get; set; }
        public bool IsPrefferedService { get; set; }

        [Required(ErrorMessage = "Введите ID категории")]
        [Display(Name = "ID Категории")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}