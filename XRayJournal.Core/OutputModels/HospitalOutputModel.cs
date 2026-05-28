using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.OutputModels
{
    public class HospitalOutputModel
    {
        public int Id { get; set; }

        public string DepName { get; set; }

        public string Clinic { get; set; }

        public string Department { get; set; }

        public List<CabinetOutputModel> Cabinets { get; set; } = new List<CabinetOutputModel>();
    }
}
