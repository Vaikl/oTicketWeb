using System.ComponentModel.DataAnnotations;

namespace Clinic.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите логин")]
        [Display(Name = "Логин")]
        [StringLength(25)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите свое имя")]
        [Display(Name = "Имя")]
        [StringLength(25)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введите свою фамилию")]
        [Display(Name = "Фамилия")]
        [StringLength(25)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введите свое отчество")]
        [Display(Name = "Отчество")]
        [StringLength(25)]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Введите номер телефона")]
        [Display(Name = "Телефон")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Введите адрес электронной почты")]
        [Display(Name = "Электронная почта")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}