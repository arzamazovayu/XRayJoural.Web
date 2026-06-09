using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.ReportsModels
{
    public class YearlyReportModel
    {
        public string CabinetNum { get; set; }

        public string Modality { get; set; }

        public int AreaCode { get; set; }

        public string AreaName { get; set; }

        public int Count { get; set; }
    }
}
