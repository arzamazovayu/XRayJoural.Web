using Microsoft.EntityFrameworkCore;
using XRayJournal.Core;
using XRayJournal.Core.DTOs;
using XRayJournal.Core.IRepositories;


namespace XRayJournal.DAL
{
    public class PatientRepository : IPatientRepository
    {
        public DataContext _dataContext;
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
    }
}
