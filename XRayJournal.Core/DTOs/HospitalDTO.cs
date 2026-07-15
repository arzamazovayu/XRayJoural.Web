using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.DTOs
{
    public class HospitalDTO
    {
        public int Id { get; set; }

        //public string DepName { get; set; }

        public string Clinic { get; set; }

        public List<CabinetDTO>? Cabinets { get; set; }

        public List<DepartmentDTO>? Departments { get; set; }

    }
}
