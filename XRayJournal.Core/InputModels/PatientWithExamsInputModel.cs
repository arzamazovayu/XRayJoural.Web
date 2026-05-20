using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.InputModels
{
    public class PatientWithExamsInputModel
    {
        public PatientInputModel Patient { get; set; } = new PatientInputModel();

        public List<XRayExamInputModel> Exams { get; set; } = new List<XRayExamInputModel>();

        public NumberInputModel Number { get; set; } = new NumberInputModel();
    }
}
