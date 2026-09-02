using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        
        public string Login { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public UserRole Role { get; set; }

        public int? CabinetId { get; set; }

        public bool IsAuthenticated { get; set; }

        public bool RequiresPasswordChange { get; set; }
    }
}
