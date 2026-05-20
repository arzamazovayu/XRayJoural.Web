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

        public DbSet<XRayExamDTO> Exams { get; set; }

        public DbSet<UserDTO> Users { get; set; }

        public DbSet<NumberDTO> Numbers {  get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string connectionString = Options.ConnectionString;
            string connectionString = "Server = localhost; Port = 5432; User Id = postgres; Password = Flvby1; Database = XRayJournalWeb";
            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PatientDTO>() //Пациент - Исследование один ко многим
                .HasMany(p => p.Exams)
                .WithOne(e => e.Patient)
                .HasForeignKey(e => e.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PatientDTO>() //Пациент - Номер один ко многим
                .HasMany(p => p.Numbers)
                .WithOne(n => n.Patient)
                .HasForeignKey(n => n.PatientId);
        }
    }
}
