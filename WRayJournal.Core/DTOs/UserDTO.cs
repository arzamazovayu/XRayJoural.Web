using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WRayJournal.Core;

namespace XRayJournal.Core.DTOs
{
    public class UserDTO
    {
        public int ID {  get; set; }

        public string? Login { get; set; }

        public string? Password { get; set; }

        public UserRole Role { get; set; }

        public List<PatientDTO> Patients { get; set; }
    }
}
