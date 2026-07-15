using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.OutputModels
{
    public class DepartmentOutputModel
    {
        public int Id { get; set; }

        public string DepName { get; set; }

        public string FullName { get; set; }

        public int HospitalId { get; set; }
    }
}
