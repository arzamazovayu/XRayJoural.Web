using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XRayJournal.BLL;
using XRayJournal.Core.InputModels;
using XRayJournal.Core.OutputModels;

namespace XRayJoural.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController:ControllerBase
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

        [Authorize(Roles = "Doctor, Laborant")]
        [HttpGet("all", Name ="Все пациенты")]
        public ActionResult<IEnumerable<PatientOutputModel>> GetAll()
        {
            return _patientService.GetAll();
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("{id}")]
        public ActionResult<PatientOutputModel> GetById(int id)
        {
            try
            {
                var result = GetPatientById(id);
                return Ok(result);
            }
            catch(InvalidOperationException)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "Doctor")]
        private PatientOutputModel GetPatientById(int id)
        {
            var tmp = _patientService.GetAll();
            var result = tmp.Single(p => p.Id == id);
            return result;
        }

        [Authorize(Roles = "Doctor, Laborant")]
        [HttpPost]
        public ActionResult<PatientOutputModel> Add(PatientInputModel patient)
        {
            var result = _patientService.Add(patient);

            return Ok(result);
        }
    }
}
