using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.InputModels
{
    public class NumberInputModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Порядковый номер обязателен!")]
        [Range(1, 99999, ErrorMessage = "Порядковый номер должен быть от 1 до 99 999")]
        public int YearlyNum { get; set; }

        [Required(ErrorMessage = "Ежедневный номер обязателен!")]
        [Range(1, 999, ErrorMessage = "Ежедневный номер должен быть от 1 до 999")]
        public int DailyNum { get; set; }

        //public string DisplayNumber => $"{YearlyNum}/{DailyNum}";
    }
}
