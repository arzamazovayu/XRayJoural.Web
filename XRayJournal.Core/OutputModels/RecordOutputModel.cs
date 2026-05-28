using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.OutputModels
{
    public class RecordOutputModel
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }

        public string DisplayNumber { get; set; }

        public PatientOutputModel Patient { get; set; }

        public XRayExamOutputModel Exam { get; set; }

        public NumberOutputModel Number { get; set; }

        public string UserFIO { get; set; }

    }
}
