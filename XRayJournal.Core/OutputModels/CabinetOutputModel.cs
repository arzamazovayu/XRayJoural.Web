using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.OutputModels
{
    public class CabinetOutputModel
    {
        public int Id { get; set; }

        public string CabinetName { get; set; }

        public string? Clinic { get; set; }

        public string? CabNum { get; set; }

        public string? Modality { get; set; }

        public string? Type { get; set; }

        public int? IdClinic { get; set; }

    }
}
