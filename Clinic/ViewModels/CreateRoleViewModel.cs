using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clinic.ViewModels
{
    public class CreateRoleViewModel
    {

        [Required(ErrorMessage = "Введите название роли")]
        [Display(Name = "Роль")]
        public string Name { get; set; }

        public CreateRoleViewModel()
        {
      
        }
    }
}