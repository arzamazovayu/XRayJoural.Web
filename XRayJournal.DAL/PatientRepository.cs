using Mapster;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using XRayJournal.Core;
using XRayJournal.Core.DTOs;
using XRayJournal.Core.InputModels;
using XRayJournal.Core.IRepositories;


namespace XRayJournal.DAL
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DataContext _dataContext;
        public PatientRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public PatientDTO Add(PatientDTO patient)
        {
            _dataContext.Patients.Add(patient);
            _dataContext.SaveChanges();
            return patient;
        }

        public List<PatientDTO> GetAll()
        {
            var result = _dataContext.Patients.OrderBy(p=>p.Id).ToList();
            return result;
        }

        public PatientDTO GetById(int id)
        {
            var result = _dataContext.Patients.Single(p => p.Id == id);
            return result;
        }

        public List<PatientDTO> GetAllWithExams()
        {
            var result = _dataContext.Patients
                .Where(p => !p.IsDeleted)
                //.Include(p => p.Exams)
                //.Include(p => p.Numbers)
                .OrderBy(p => p.Id)
                .ToList();
            return result;
        }

        public PatientDTO Update(PatientDTO patient)
        {
            var exist = _dataContext.Patients.Find(patient.Id);
            if (exist == null)
            {
                return null;
            }

            exist.SecondName = patient.SecondName;
            exist.FirstName = patient.FirstName;
            exist.ThirdName = patient.ThirdName;
            exist.BirthDate = patient.BirthDate;
            exist.Sex = patient.Sex;
            exist.MedNumber = patient.MedNumber;

            _dataContext.SaveChanges();
            return exist;
        }

        public bool Delete(int id) 
        {
            var patient = _dataContext.Patients.Find(id);
            if (patient == null)
            {
                return false;
            }

            patient.IsDeleted = true;
            
            _dataContext.SaveChanges();
            return true;
        }

        public bool Restore(int id) 
        {
            var patient = _dataContext.Patients.Find(id);

            if (patient == null || !patient.IsDeleted)
            {
                return false;
            }

            patient.IsDeleted = false;
            _dataContext.SaveChanges();
            return true;
        }

        public PatientDTO GetPatientWithExamsAndNumbersById(int id)
        {
            var patient = _dataContext.Patients
                .Where(p => p.Id == id && !p.IsDeleted)
                //.Include(p => p.Exams)
                //.Include(p => p.Numbers)
                //.OrderByDescending(p => p.Exams.Max(e => e.XRayDate))
                .FirstOrDefault();
                
            return patient;
        }

        public async Task<PatientDTO?> GetByMedNumberAsync(string medId)
        {
            return await _dataContext.Patients.FirstOrDefaultAsync(p => p.MedNumber == medId);
        }

        public async Task<List<PatientDTO>> FindPatientsAsync(PatientSearchInputModel search)
        {
            var query = _dataContext.Patients.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search.SecondName))
                query = query.Where(p => p.SecondName == search.SecondName);
            if (!string.IsNullOrWhiteSpace(search.FirstName))
                query = query.Where(p => p.FirstName == search.FirstName);
            if (!string.IsNullOrWhiteSpace(search.ThirdName))
                query = query.Where(p => p.ThirdName == search.ThirdName);
            if (search.BirthDate.HasValue && search.BirthDate != DateOnly.FromDateTime(DateTime.Now))
                query = query.Where(p => p.BirthDate == search.BirthDate.Value);
            if (!string.IsNullOrWhiteSpace(search.MedNumber))
                query = query.Where(p => p.MedNumber == search.MedNumber);
            return await query.ToListAsync();
        }
    }
}
