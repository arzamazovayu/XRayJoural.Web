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

        public async Task<OperationResult<RecordOutputModel>> GetByIdAsync (int id)
        {
            try
            {
                var record = _recordRepository.GetByIdAsync(id);
                if (record == null)
                {
                    return OperationResult<RecordOutputModel>.Fail("Запись не найдена");
                }
                var result = record.Adapt<RecordOutputModel>();
                return OperationResult<RecordOutputModel>.Ok(result);
            }
            catch (Exception ex)
            {
                return OperationResult<RecordOutputModel>.Fail($"Ошибка поиска записи: {ex.Message}");
            }
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

        public async Task<OperationResult<RecordOutputModel>> CreateRecordAsync(RecordInputModel input, int userId, int cabinetId)
        {
            try
            {
                //Получение/создание пациента
                var patient = await GetOrCreatePatientAsync(input.Patient);

                //Создание исследования с привязкой к кабинету
                var examDto = input.Exam.Adapt<XRayExamDTO>();
                examDto.IdCabinet = cabinetId;
                var exam = _examRepository.Add(examDto);

                //Получение/создание номера
                var (yearly, daily) = await _numberService.CalculateNextNumberAsync(input.Date, cabinetId);
                Console.WriteLine($"[DEBUG] Calculated number: {yearly}/{daily} for cabinet {cabinetId}"); //ОТЛАДКА
                var number = new NumberDTO
                {
                    YearlyNum = yearly,
                    DailyNum = daily,
                    XRayDate = input.Date,
                    PatientId = patient.Id
                };
                var savedNumber = await _numberRepository.AddAsync(number);
                Console.WriteLine($"[DEBUG] Saved number with ID: {savedNumber?.Id}");//ОТЛАДКА
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

        public async Task<OperationResult<List<RecordNecessaryOutputModel>>> GetNecessaryRecordsAsync(
            DateOnly? startDate = null, DateOnly? endDate = null)
        {
            try
            {
                //Если даты null, то текущий месяц по умолчанию
                if (!startDate.HasValue && !endDate.HasValue)
                {
                    var today = DateOnly.FromDateTime(DateTime.Now);
                    //startDate = new DateOnly(today.Year, today.Month, 1); //Начало месяца
                    //endDate = startDate.Value.AddMonths(1).AddDays(-1); // + 1 месяц - 1 день = последний день текущего месяца
                    endDate = today;
                    startDate = endDate.Value.AddMonths(-1);
                }

                var records = await _recordRepository.GetNecessaryRecordsAsync(startDate, endDate);
                return OperationResult<List<RecordNecessaryOutputModel>>.Ok(records);
            }
            catch (Exception ex)
            {
                return OperationResult<List<RecordNecessaryOutputModel>>.Fail(
                    $"Ошибка получения записей: {ex.Message}");
            }
        }

        //Метод получения записей с пагинацией (страницированием)
        public async Task<OperationResult<PagedResult<RecordNecessaryOutputModel>>> GetNecessaryRecordsPagedAsync(
            int page = 1,
            int pageSize = 50,
            DateOnly? startDate = null,
            DateOnly? endDate = null,
            string? searchText = null,
            string? sex = null)
        {
            try
            {
                var allRecords = await _recordRepository.GetNecessaryRecordsAsync(startDate, endDate);

                // Фильтрация
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    var search = searchText.Trim().ToLower();
                    allRecords = allRecords.Where(r =>
                        r.SecondName.ToLower().Contains(search) ||
                        r.FirstName.ToLower().Contains(search) ||
                        (r.ThirdName != null && r.ThirdName.ToLower().Contains(search)) ||
                        r.MedNumber.ToLower().Contains(search)
                    ).ToList();
                }

                var totalCount = allRecords.Count;
                var pagedRecords = allRecords
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var result = new PagedResult<RecordNecessaryOutputModel>
                {
                    Items = pagedRecords,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                };

                return OperationResult<PagedResult<RecordNecessaryOutputModel>>.Ok(result);
            }
            catch (Exception ex)
            {
                return OperationResult<PagedResult<RecordNecessaryOutputModel>>.Fail(
                    $"Ошибка получения записей: {ex.Message}");
            }
        }
    }
}
