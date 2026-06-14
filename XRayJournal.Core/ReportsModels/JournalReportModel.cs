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

        public List<JournalPatientGroup> PatientGroups { get; set; } = new();

        public JournalDaySummary Summary { get; set; } = new();
    }

    public class JournalPatientGroup
    {
        public string DisplayNumber { get; set; }

        public string PatientFIO { get; set; }

        public string BirthDate { get; set; }

        public string MedNumber { get; set; }

        public string Category { get; set; }

        public string DepName { get; set; }

        public List<JournalExamItem> Exams { get; set; } = new();
    }

    public class JournalExamItem
    {
        public string ExamName { get; set; }

        public int Shots { get; set; }

        public float Dose { get; set; }

    }
    public class JournalDaySummary
    {
        public int PatientCount { get; set; }

        public string Doctor { get; set; }

        public string Laborant { get; set; }

        public Dictionary<string, int> CategoryCounts { get; set; } = new();

        public int TotalExams { get; set; }

        public int TotalShots { get; set; }
    }
}
