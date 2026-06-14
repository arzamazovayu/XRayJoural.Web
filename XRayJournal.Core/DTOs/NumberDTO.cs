using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.DTOs
{
    public class NumberDTO
    {
        public int Id {  get; set; }

        public int YearlyNum { get; set; }

        public int DailyNum { get; set; }

        public DateOnly XRayDate { get; set; }

        public int PatientId { get; set; }

        public virtual List<RecordDTO>? Records { get; set; }

    }
}

