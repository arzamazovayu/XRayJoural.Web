using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.OutputModels
{
    public class XrayExamNecessaryInfoOutputModel
    {
        public int Id { get; set; }

        public string XRayName { get; set; }

        public float XRayDose { get; set; }

        public DateOnly XRayDate { get; set; }

        public string Category { get; set; }

        //public string Clinic { get; set; }

    }
}
