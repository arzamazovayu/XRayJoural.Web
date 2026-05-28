using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.InputModels
{
    public class RecordInputModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Пациент обязателен")]
        public PatientInputModel Patient { get; set; } = new PatientInputModel();

        [Required(ErrorMessage = "Исследование обязательно")]
        public XRayExamInputModel Exam { get; set; } = new XRayExamInputModel();

        [Required(ErrorMessage = "Номер обязателен")]
        public NumberInputModel Number { get; set; } = new NumberInputModel();

        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    }
}
