using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.OutputModels
{
    public class PatientWithExamOutputModel
    {
        public int Id { get; set; }

        public string SecondName { get; set; }

        public string FirstName { get; set; }

        public string ThirdName { get; set; }

        public DateOnly BirthDate { get; set; }

        public string Sex { get; set; }

        public string MedNumber { get; set; }

        public List<XRayExamOutputModel> Exams { get; set; } = new List<XRayExamOutputModel>();
    }
}
