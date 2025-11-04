using XRayJournal.Core.IRepositories;
using XRayJournal.Core.OutputModels;
using Mapster;

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
    }
}
