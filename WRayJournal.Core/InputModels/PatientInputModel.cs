using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.InputModels
{
    public class PatientInputModel
    {
        [Required(ErrorMessage ="Фамилия обязательна!")]
        [StringLength(255, MinimumLength = 1, ErrorMessage ="Фамилия минимум 1 символ!")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "Имя обязательно!")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Имя минимум 1 символ!")]
        public string FirstName { get; set; }

        [StringLength(255, MinimumLength = 1, ErrorMessage = "Отчество минимум 1 символ!")]
        public string ThirdName { get; set; }

        public DateOnly BirthDate { get; set; }

        public string Sex { get; set; }

        [StringLength(20, MinimumLength = 3, ErrorMessage = "Номер минимум 3 символа!")]
        [Required(ErrorMessage = "Номер карты обязателен!")]
        public string MedNumber { get; set; }
    }
}
