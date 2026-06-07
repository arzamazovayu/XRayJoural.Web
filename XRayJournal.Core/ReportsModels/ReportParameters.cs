using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.ReportsModels
{
    public class ReportParameters
    {
        public List<int> CabinetIds { get; set; } = new();

        public List<int> UserIds { get; set; } = new();

        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public bool ByCabinet { get; set; } = true; //true – по кабинетам, false – по сотрудникам
    }
}
