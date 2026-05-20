using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core
{
    internal class Options
    {
        public static string ConnectionString
        {
            get
            {
                return Environment.GetEnvironmentVariable("postgres");
            }
        }
    }
}
