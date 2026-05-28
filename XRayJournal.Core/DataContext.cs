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

        public DbSet<RecordDTO> Records {  get; set; }

        public DbSet<CabinetDTO> Cabinets { get; set; }

        public DbSet<HospitalDTO> Hospitals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string connectionString = Options.ConnectionString;
            string connectionString = "Server = localhost; Port = 5432; User Id = postgres; Password = Flvby1; Database = XRayJournalWeb";
            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PatientDTO>() //Запись Э- Пациент
                .HasMany(p => p.Records)
                .WithOne(r => r.Patient)
                .HasForeignKey(r => r.PatientId);

            modelBuilder.Entity<NumberDTO>() //Запись Э- Номер
                .HasMany(n => n.Records)
                .WithOne(r => r.Number)
                .HasForeignKey(r => r.NumberId);

            modelBuilder.Entity<XRayExamDTO>() //Запись Э- Исследования
                .HasMany(e => e.Records)
                .WithOne(r => r.Exam)
                .HasForeignKey(r => r.ExamId);

            modelBuilder.Entity<XRayExamDTO>() //Кабинет -Е Исследования
                .HasOne(e => e.Cabinet)
                .WithMany(c => c.Exams)
                .HasForeignKey(e => e.IdCabinet);

            modelBuilder.Entity<HospitalDTO>(entity =>
            {
                entity.HasKey(h => h.Id);
                entity.Property(h => h.Id).ValueGeneratedOnAdd();

                entity.HasMany(h => h.Cabinets) //Кабинет Э- Больница
                    .WithOne(c => c.Hospital)
                    .HasForeignKey(c => c.IdClinic);
            });

            modelBuilder.Entity<CabinetDTO>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();

                entity.HasOne(c => c.Hospital) //Больница -Е Кабинет
                    .WithMany(h => h.Cabinets)
                    .HasForeignKey(c => c.IdClinic);

                entity.HasMany(c => c.Exams) //Исследоание Э- Кабинет
                    .WithOne(e => e.Cabinet)
                    .HasForeignKey(e => e.IdCabinet);

                entity.HasMany(c => c.Users) //Пользователь Э- Кабинет
                    .WithOne(u => u.Cabinet)
                    .HasForeignKey(u => u.CabinetId);
            });

            modelBuilder.Entity<UserDTO>(entity =>
            {
                entity.HasKey(u => u.ID);
                entity.Property(u => u.ID).ValueGeneratedNever();

                entity.HasOne(u => u.Cabinet) //Кабинет -Е Пользователь
                    .WithMany(c => c.Users)
                    .HasForeignKey(u => u.CabinetId);

                entity.HasMany(u => u.Records) //Запись Э- Пользователь
                    .WithOne(r => r.User)
                    .HasForeignKey(r => r.UserId);
            });

            modelBuilder.Entity<RecordDTO>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).ValueGeneratedOnAdd();

                entity.HasOne(r => r.Patient) //Пациент -Е Запись
                .WithMany(p => p.Records)
                .HasForeignKey(r => r.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Number) //Номер -Е Запись
                .WithMany(p => p.Records)
                .HasForeignKey(r => r.NumberId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Exam) //Исследоание -Е Запись
                .WithMany(p => p.Records)
                .HasForeignKey(r => r.ExamId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.User) //Пользователь -Е Запись
                .WithMany(p => p.Records)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
