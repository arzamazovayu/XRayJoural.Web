using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        public PatientOutputModel Get()   // 1:55:55
        {
            return new PatientOutputModel()
            {
                Id = 1,
                SecondName = "Осипов",
                FirstName = "Евгений",
                ThirdName = "Игнатович",
                BirthDate = System.DateOnly.Parse("01.01.1991"),
                Sex = "Муж",
                MedNumber = "25-002385",
            };
        }
    }
}
