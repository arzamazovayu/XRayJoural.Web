using XRayJournal.Core.IRepositories;
using XRayJournal.Core.OutputModels;
using XRayJournal.Core.InputModels;
using Mapster;
using XRayJournal.Core.DTOs;
using XRayJournal.Core.Results;

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

        public OperationResult<PatientOutputModel> GetById(int id)
        {
            try
            {
                var patient = _patientRepository.GetById(id);
                if (patient == null)
                {
                    return OperationResult<PatientOutputModel>.Fail("Пациент не найден");
                }

                var outputModel = patient.Adapt<PatientOutputModel>();
                return OperationResult<PatientOutputModel>.Ok(outputModel);
            }
            catch (Exception ex)
            {
                return OperationResult<PatientOutputModel>.Fail($"Ошибка при получении пациента: {ex.Message}");
            }
        }

        public OperationResult<PatientOutputModel> Add(PatientInputModel patient) 
        {
            try
            {
                var patientDto = patient.Adapt<PatientDTO>();
                var result = _patientRepository.Add(patientDto);
                var outputModel = result.Adapt<PatientOutputModel>();
                return OperationResult<PatientOutputModel>.Ok(outputModel);
            }
            catch (Exception ex)
            {
                return OperationResult<PatientOutputModel>.Fail($"Ошибка при добавлении пациента: {ex.Message}");
            }
        }

        public  List<PatientWithExamOutputModel> GetAllWithExams()
        {
            var tmp = _patientRepository.GetAllWithExams();
            var result = tmp.Adapt<List<PatientWithExamOutputModel>>();
            return result;
        }

        public OperationResult<PatientOutputModel> Update(PatientInputModel patient)
        {
            try
            {
                if (patient.Id == 0)
                {
                    return OperationResult<PatientOutputModel>.Fail("Id пациента не указан");
                }

                var patientDto = patient.Adapt<PatientDTO>();
                var updatedPatient = _patientRepository.Update(patientDto);

                if (updatedPatient == null)
                {
                    return OperationResult<PatientOutputModel>.Fail("Пациент не найден");
                }

                var outputModel = updatedPatient.Adapt<PatientOutputModel>();
                return OperationResult<PatientOutputModel>.Ok(outputModel);
            }
            catch (Exception ex)
            {
                return OperationResult<PatientOutputModel>.Fail($"Ошибка при добавлении пациента: {ex.Message}");
            }
        }

        public OperationResult Delete(int id)
        {
            try
            {
                var success = _patientRepository.Delete(id);
                if (!success)
                {
                    return OperationResult.Fail("Пациент не найден!");
                }           
                return OperationResult.Ok();
                
            }
            catch (Exception ex)
            {
                return OperationResult.Fail($"Ошибка при удалении пациента: {ex.Message}");
            }
        }

        public OperationResult Restore(int id)
        {
            try
            {
                var success = _patientRepository.Restore(id);
                if (!success)
                {
                    return OperationResult.Fail("Пациент не найден!");
                }
                return OperationResult.Ok();

            }
            catch (Exception ex)
            {
                return OperationResult.Fail($"Ошибка при удалении пациента: {ex.Message}");
            }
        }

        public OperationResult<PatientWithExamOutputModel> GetPatientWithExamsAndNumbersById(int id)
        {
            try
            {
                var result = _patientRepository.GetPatientWithExamsAndNumbersById(id);
                if (result == null)
                {
                    return OperationResult<PatientWithExamOutputModel>.Fail("Пациент не найден");
                }
                var outputModel = result.Adapt<PatientWithExamOutputModel>();
                return OperationResult<PatientWithExamOutputModel>.Ok(outputModel);
            }
            catch (Exception ex)
            {
                return OperationResult<PatientWithExamOutputModel>.Fail($"Ошибка при получении пациента: {ex.Message}");
            }
        }

        public DateOnly GetPatientsLastExamDate(int id)
        {            
            return _patientRepository.GetPatientsLastExamDate(id);
        }
    }
}
