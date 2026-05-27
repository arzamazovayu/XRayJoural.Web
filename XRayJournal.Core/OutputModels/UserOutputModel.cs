using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.OutputModels
{
    public class UserOutputModel
    {
        public int Id { get; set; }

        public string FIO { get; set; }

        public string FIOshort { get; set; }

        public UserRole Role { get; set; }
    }
}
