using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.ReportsModels
{
    public class DailyReportModel
    {
        public string EntityName { get; set; }

        public int PatientCount { get; set; }

        public int ExamCount { get; set; }

        public decimal TotalCost { get; set; }

        public DateOnly Date { get; set; }
    }
}
