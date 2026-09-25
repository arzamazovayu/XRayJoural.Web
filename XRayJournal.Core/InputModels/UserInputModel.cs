using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace XRayJournal.Core.InputModels
{
    public class UserInputModel
    {
        [Required(ErrorMessage = "Фамилия обязательна")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Фамилия от 1 до 100 символов")]
        public string SecondName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Имя от 1 до 100 символов")]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Отчество до 100 символов")]
        public string? ThirdName { get; set; }

        [Required(ErrorMessage = "Логин обязателен")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Логин от 3 до 50 символов")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Логин может содержать только латинские буквы, цифры, точку, дефис и подчёркивание")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Выберите кабинет")]
        [Range(1, int.MaxValue, ErrorMessage = "Некорректный кабинет")]
        public int CabinetId { get; set; }

        [Required(ErrorMessage = "Выберите должность")]
        public UserRole? Role { get; set; }
    }
}
