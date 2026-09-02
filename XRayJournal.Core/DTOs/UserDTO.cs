using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core;

namespace XRayJournal.Core.DTOs
{
    public class UserDTO
    {
        public int ID { get; set; }

        public string? Login { get; set; }

        public string PwHash { get; set; }

        public UserRole Role { get; set; }

        public string FIO { get; set; }

        public string FIOshort { get; set; }

        public int? CabinetId { get; set; }

        public DateTime PwDate { get; set; }

        public virtual List<CabinetDTO> Cabinets { get; set; }

        public virtual List<RecordDTO>? Records { get; set; } 
    }
}
