using XRayJournal.Core.IRepositories;
using XRayJournal.Core.OutputModels;
using XRayJournal.Core.InputModels;
using Mapster;
using XRayJournal.Core.DTOs;

namespace XRayJournal.BLL
{
    public class PatientService
    {
        public IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public List<PatientOutputModel> GetAll()
        {
            var tmp = _patientRepository.GetAll();
            var result = tmp.Adapt<List<PatientOutputModel>>();
            return result;
        }

        public PatientOutputModel Add(PatientInputModel patient) 
        {
            var tmp = _patientRepository.Add(patient.Adapt<PatientDTO>());
            var result = tmp.Adapt<PatientOutputModel>();
            return result;
        }
    }
}
