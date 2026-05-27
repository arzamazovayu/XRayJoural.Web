using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.Models
{
    public class UserModel
    {
        public string Login { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public UserRole Role { get; set; }

        public string FIO { get; set; }

        public string FIOshort { get; set; }

        public bool IsAuthenticated { get; set; }
    }
}
