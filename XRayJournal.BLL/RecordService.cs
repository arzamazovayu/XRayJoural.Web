using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core.IRepositories;
using XRayJournal.Core.OutputModels;
using XRayJournal.Core.InputModels;
using Mapster;
using XRayJournal.Core.DTOs;
using XRayJournal.Core.Results;

namespace XRayJournal.BLL
{
    public class RecordService
    {
        private readonly IRecordRepository _recordRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IXRayExamRepository _examRepository;
        private readonly INumberRepository _numberRepository;
        private readonly NumberService _numberService;

        public RecordService(
            IRecordRepository recordRepository, 
            IPatientRepository patientRepository, 
            IUserRepository userRepository, 
            IXRayExamRepository examRepository, 
            INumberRepository numberRepository, 
            NumberService numberService)
        {
            _recordRepository = recordRepository;
            _patientRepository = patientRepository;
            _userRepository = userRepository;
            _examRepository = examRepository;
            _numberRepository = numberRepository;
            _numberService = numberService;
        }

        private async Task<PatientDTO> GetOrCreatePatientAsync(PatientInputModel input)
        {
            //Поиск по номеру карты
            var existing = _patientRepository.GetByMedNumber(input.MedNumber);

            if (existing != null)
                return existing;

            //Создание нового
            var newPatient = input.Adapt<PatientDTO>();
            return _patientRepository.Add(newPatient);
        }

        public async Task<OperationResult<RecordOutputModel>> CreateRecordAsync(RecordInputModel input, int userId)
        {
            try
            {
                //Получение/создание пациента
                var patient = await GetOrCreatePatientAsync(input.Patient);

                //Создание исследования
                var examDto = input.Exam.Adapt<XRayExamDTO>();
                examDto.PatientId = patient.Id;
                var exam = _examRepository.Add(examDto);

                //Получение/создание номера
                var (yearly, daily) = await _numberService.CalculateNextNumberAsync(input.Date);
                var number = new NumberDTO
                {
                    YearlyNum = yearly,
                    DailyNum = daily,
                    XRayDate = input.Date,
                    PatientId = patient.Id
                };
                var savedNumber = await _numberRepository.AddAsync(number);

                //Создание записи
                var record = new RecordDTO
                {
                    PatientId = patient.Id,
                    ExamId = exam.Id,
                    NumberId = savedNumber.Id,
                    UserId = userId,
                    Date = input.Date
                };

                var created = await _recordRepository.AddAsync(record);
                var result = created.Adapt<RecordOutputModel>();

                return OperationResult<RecordOutputModel>.Ok(result);
            }
            catch (Exception ex)
            {
                return OperationResult<RecordOutputModel>.Fail($"Ошибка создания записи: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<RecordOutputModel>>> GetRecordsByDateRangeAsync(DateOnly start, DateOnly end)
        {
            try
            {
                var records = await _recordRepository.GetByDateRangeAsync(start, end);
                var result = records.Adapt<List<RecordOutputModel>>();
                return OperationResult<List<RecordOutputModel>>.Ok(result);
            }
            catch (Exception ex)
            {
                return OperationResult<List<RecordOutputModel>>.Fail($"Ошибка получения записей: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<RecordOutputModel>>> GetAllRecordsAsync()
        {
            try
            {
                var records = await _recordRepository.GetAllAsync();
                var result = records.Adapt<List<RecordOutputModel>>();
                return OperationResult<List<RecordOutputModel>>.Ok(result);
            }
            catch (Exception ex)
            {
                return OperationResult<List<RecordOutputModel>>.Fail($"Ошибка получения записей: {ex.Message}");
            }
        }
    }
}
