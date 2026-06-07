using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.ReportsModels
{
    public class JournalReportModel
    {
        public DateOnly Date { get; set; }
        public string DisplayNumber { get; set; }
        public string PatientFullName { get; set; }
        public string PatientBirthDate { get; set; }
        public string MedNumber { get; set; }
        public string Category { get; set; }
        public string Department { get; set; }
        public string ExamName { get; set; }
        public int Shots { get; set; }
        public float Dose { get; set; }
        // Флаги для объединения ячеек
        public bool IsFirstInDate { get; set; }
        public bool IsFirstForPatient { get; set; }
    }
}
