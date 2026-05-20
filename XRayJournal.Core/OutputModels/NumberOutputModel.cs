using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.OutputModels
{
    public class NumberOutputModel
    {
        public int YearlyNum {  get; set; }

        public int DailyNum { get; set; }

        public string DisplayNumber => $"{YearlyNum}/{DailyNum}";
    }
}
