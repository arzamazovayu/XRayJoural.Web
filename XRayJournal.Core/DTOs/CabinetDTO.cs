using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.DTOs
{
    public class CabinetDTO
    {
        public int Id { get; set; }

        public string CabinetName { get; set; }

        public string? Clinic { get; set; }

        public string? CabNum { get; set; }

        public string? Modality { get; set; }

        public string? Type { get; set; }

        public int? IdClinic { get; set; }

        public HospitalDTO? Hospital { get; set; }

        public virtual List<XRayExamDTO> Exams { get; set; }

        public virtual List<UserDTO> Users { get; set; }

    }
}
