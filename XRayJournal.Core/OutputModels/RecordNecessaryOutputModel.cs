using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.OutputModels
{
    public class RecordNecessaryOutputModel
    {
        public int Id { get; set; }

        //Данные номера
        public int YearlyNum { get; set; }

        public int DailyNum { get; set; }

        public string DisplayNumber => $"{YearlyNum}/{DailyNum}";

        //Данные пациента
        public string SecondName { get; set; }

        public string FirstName { get; set; }

        public string ThirdName { get; set; }

        public DateOnly BirthDate { get; set; }

        public string MedNumber { get; set; }

        //Данные исследования
        public string? XRayName { get; set; }

        public int? XRayShots { get; set; }

        public float? XRayDose { get; set; }

        public DateOnly? XRayDate { get; set; }

        public string? Category { get; set; }

        //Данные клиники и отделения
        public string? DepName { get; set; }

        //Данные записи
        public DateOnly RecordDate { get; set; }
    }
}
