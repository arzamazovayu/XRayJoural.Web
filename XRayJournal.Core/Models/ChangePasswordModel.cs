using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Введите логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите текущий пароль")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Введите новый пароль")]
        [MinLength(6, ErrorMessage = "Пароль должен быть не менее 6 символов")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare(nameof(NewPassword), ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
