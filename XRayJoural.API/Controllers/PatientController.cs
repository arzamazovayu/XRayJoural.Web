using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XRayJournal.BLL;
using XRayJournal.Core.InputModels;
using XRayJournal.Core.OutputModels;
using XRayJournal.Core.Results;

namespace XRayJoural.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PatientController : ControllerBase
    {
        /// <summary>
        /// Тестовое описание метода
        /// </summary>
        /// <returns>Возвращает мок модель пациента, чтобы это ни значило</returns>
        /// <response code="200">Возвращает мок модель пациента, чтобы это ни значило</response>

        private PatientService _patientService;

        public PatientController(PatientService patientService)
        {
            _patientService = patientService;
        }

        /// <summary>
        /// Получить всех пациентов (без исследований)
        /// </summary>
        [Authorize(Roles = "Doctor, Laborant")]
        [HttpGet("all", Name = "Все пациенты")]
        public ActionResult<IEnumerable<PatientOutputModel>> GetAll()
        {
            return _patientService.GetAll();
        }

        /// <summary>
        /// Получить пациента по ID
        /// </summary>
        [Authorize(Roles = "Doctor, Laborant")]
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<PatientOutputModel>> GetById(int id)
        {
            try
            {
                var result = _patientService.GetById(id);
                if (!result.Success)
                {
                    return NotFound(new ApiResponse<PatientOutputModel>
                    {
                        Success = false,
                        ErrorMessage = result.ErrorMessage
                    });
                }
                return Ok(new ApiResponse<PatientOutputModel>
                {
                    Success = true,
                    Data = result.Data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<PatientOutputModel>
                {
                    Success = false,
                    ErrorMessage = $"Ошибка при получении пациента: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Получить всех пациентов с исследованиями
        /// </summary>
        [Authorize(Roles = "Doctor, Laborant")]
        private ActionResult<ApiResponse<List<PatientWithExamOutputModel>>> GetAllWithExams()
        {
            try
            {
                var patients = _patientService.GetAllWithExams();

                return Ok(new ApiResponse<List<PatientWithExamOutputModel>>
                {
                    Success = true,
                    Data = patients
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<PatientWithExamOutputModel>>
                {
                    Success = false,
                    ErrorMessage = $"Ошибка при получении пациентов с исследованиями: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Добавить нового пациента
        /// </summary>
        [Authorize(Roles = "Doctor, Laborant")]
        [HttpPost]
        public ActionResult<ApiResponse<PatientOutputModel>> Add(PatientInputModel patient)
        {
            try
            {
                var result = _patientService.Add(patient);

                return Ok(new ApiResponse<PatientOutputModel>
                {
                    Success = true,
                    Data = result.Data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<PatientOutputModel>
                {
                    Success = false,
                    ErrorMessage = $"Ошибка при добавлении пациента: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Обновить данные пациента
        /// </summary>
        [Authorize(Roles = "Doctor,Laborant")]
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<PatientOutputModel>> Update(int id, PatientInputModel patient)
        {
            try
            {
                patient.Id = id;
                var result = _patientService.Update(patient);

                if (!result.Success)
                {
                    return BadRequest(new ApiResponse<PatientOutputModel>
                    {
                        Success = false,
                        ErrorMessage = result.ErrorMessage
                    });
                }

                return Ok(new ApiResponse<PatientOutputModel>
                {
                    Success = true,
                    Data = result.Data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<PatientOutputModel>
                {
                    Success = false,
                    ErrorMessage = $"Ошибка при обновлении пациента: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Удалить пациента (только для врачей)
        /// </summary>
        [Authorize(Roles = "Doctor")]
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse> Delete(int id)
        {
            try
            {
                var result = _patientService.Delete(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    ErrorMessage = $"Ошибка при удалении пациента: {ex.Message}"
                });
            }
        }
    }
    public class ApiResponse
    {
        public bool Success { get; set; }
        public object? Data { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
