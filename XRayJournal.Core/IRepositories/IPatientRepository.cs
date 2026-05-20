using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core.DTOs;

namespace XRayJournal.Core.IRepositories
{
    public interface IPatientRepository
    {
        public List<PatientDTO> GetAll();

        public PatientDTO Add(PatientDTO patient);

        public List<PatientDTO> GetAllWithExams();

        public PatientDTO Update(PatientDTO patient);

        public bool Delete(int id);

        public PatientDTO GetById(int id);

        public bool Restore(int id);
    }
}
