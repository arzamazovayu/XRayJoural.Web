using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.ReportsModels
{
    public class RadiationReportModel
    {
        public int Counter { get; set; }

        public DateOnly Date {  get; set; }

        public string ExamName { get; set; }

        public float Dose { get; set; }

        public string Note { get; set; }
    }
}
