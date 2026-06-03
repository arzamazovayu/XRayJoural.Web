using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.InputModels
{
    public class PatientSearchInputModel
    {
        public string? SecondName { get; set; }

        public string? FirstName { get; set; }

        public string? ThirdName { get; set; }

        public DateOnly? BirthDate { get; set; }

        public string? MedNumber { get; set; }

        public string? Sex { get; set; }
    }
}
