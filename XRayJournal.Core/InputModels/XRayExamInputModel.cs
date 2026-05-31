using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.InputModels
{
    public class XRayExamInputModel
    {
        public int? Id { get; set; } //Null для новых исследований

        [Required(ErrorMessage = "Название исследования обязательно!")]
        [StringLength(200, ErrorMessage = "Название не может превышать 200 символов")]
        public string XRayName { get; set; }

        [Range(0.0001, 1000, ErrorMessage = "Доза должна быть от 0 до 1000 мЗв")]
        public float XRayDose { get; set; }

        [Range(0, 1000, ErrorMessage = "Количество снимков/последовательностей должно быть от 0 до 1000")]
        public int XRayShots { get; set; }

        [Required(ErrorMessage = "Дата исследования обязательна")]
        public DateOnly XRayDate { get; set; }

        [Required(ErrorMessage = "Категория исследования обязательна")]
        [StringLength(100, ErrorMessage = "Категория не может превышать 100 символов")]
        public string Category { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Диагноз не может превышать 500 символов")]
        public string? XRayDiagnose { get; set; }

        public bool XRayPatology { get; set; }

        [Range(0, 1000000, ErrorMessage = "Стоимость должна быть от 0 до 1 000 000")]
        public int? XRayCost { get; set; }

        [StringLength(100, ErrorMessage = "Фамилия врача не может превышать 100 символов")]
        public string? Doctor { get; set; }

        [StringLength(100, ErrorMessage = "Фамилия лаборанта не может превышать 100 символов")]
        public string? Laborant { get; set; }

        [StringLength(100, ErrorMessage ="Модальность не может превышать 100 символов")]
        public string? XRayModality { get; set; }

        public bool InOperation { get; set; }

        public bool Contrast { get; set; }

        [Required(ErrorMessage ="Кабинет обязателен!")]
        [Range(1, int.MaxValue, ErrorMessage = "Некорректный идентификатор кабинета")]
        public int CabinetId { get; set; }

        public string Side { get; set; }

        [Required(ErrorMessage = "Область исследования обязательна!")]
        public int Area { get; set; }

    }
}
