using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core.DTOs;

namespace XRayJournal.Core
{
    public class DataContext:DbContext
    {
        public DbSet<PatientDTO> Patients { get; set; }

        public DbSet<XRayExamDTO> XRayExam { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = Options.ConnectionString;
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
