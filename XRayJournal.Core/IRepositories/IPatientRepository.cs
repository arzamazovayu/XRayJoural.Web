using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core.DTOs;
using XRayJournal.Core.InputModels;

namespace XRayJournal.Core.IRepositories
{
    public interface IPatientRepository
    {
        public PatientDTO Add(PatientDTO patient);

        public PatientDTO Update(PatientDTO patient);

        public bool Delete(int id);

        public Task<PatientDTO> GetByIdAsync(int id);

        public Task<PatientDTO?> GetByMedNumberAsync(string medId);

        public Task<List<PatientDTO>> FindPatientsAsync(PatientSearchInputModel search);
    }
}
