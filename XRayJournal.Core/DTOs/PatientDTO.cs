using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.DTOs
{
    public class PatientDTO
    {
        public int Id { get; set; }

        public string SecondName { get; set; }

        public string FirstName { get; set; }

        public string? ThirdName { get; set; }

        public DateOnly BirthDate { get; set; }

        public string Sex { get; set; }

        public string MedNumber { get; set; }

        public List<XRayExamDTO>? Exams { get; set; } = new List<XRayExamDTO>();

        //Флаг удаления
        public bool IsDeleted { get; set; } = false;

        public List<NumberDTO>? Numbers { get; set; } = new List<NumberDTO>();

    }
}
