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
using Microsoft.EntityFrameworkCore;

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
                var record = await _recordRepository.GetByIdAsync(id);
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

        //private async Task<PatientDTO> GetOrCreatePatientAsync(PatientInputModel input)
        //{
        //    //Поиск по номеру карты
        //    var existing = await _patientRepository.GetByMedNumberAsync(input.MedNumber);

        //    if (existing != null)
        //    { 
        //        return existing; 
        //    }

        //    //Создание нового
        //    var newPatient = input.Adapt<PatientDTO>();
        //    return await _patientRepository.AddAsync(newPatient);
        //}

        //Метод получения записей с пагинацией (страницированием)
        public async Task<OperationResult<PagedResult<RecordNecessaryOutputModel>>> GetNecessaryRecordsPagedAsync(
            int page = 1,
            int pageSize = 50,
            DateOnly? startDate = null,
            DateOnly? endDate = null,
            string? searchText = null,
            string? sex = null,
            int? cabinetId = null)
        {
            try
            {
                var allRecords = await _recordRepository.GetNecessaryRecordsAsync(startDate, endDate, cabinetId);

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
        
        public async Task<OperationResult<RecordOutputModel>> CreateEmptyRecordAsync(
            int patientId, int numberId, int userId, DateOnly date)
        {
            try
            {
                // Проверка 1: существует ли пациент
                var patient = await _patientRepository.GetByIdAsync(patientId);
                if (patient == null)
                {
                    return OperationResult<RecordOutputModel>.Fail($"Пациент с ID {patientId} не найден");
                }

                // Проверка 2: существует ли номер
                var number = await _numberRepository.GetByIdAsync(numberId);
                if (number == null)
                {
                    return OperationResult<RecordOutputModel>.Fail($"Номер с ID {numberId} не найден");
                }

                var record = new RecordDTO
                {
                    PatientId = patientId,
                    NumberId = numberId,
                    ExamId = null,
                    UserId = userId,
                    Date = date
                };

                var created = await _recordRepository.AddAsync(record);
                var result = created.Adapt<RecordOutputModel>();
                return OperationResult<RecordOutputModel>.Ok(result);
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                return OperationResult<RecordOutputModel>.Fail($"Ошибка БД: {innerMessage}");
            }
            catch (Exception ex)
            {
                return OperationResult<RecordOutputModel>.Fail($"Ошибка создания записи: {ex.Message}");
            }
        }

        public async Task<OperationResult<RecordOutputModel>> CreateRecordForExamAsync(int patientId, int numberId, int examId, int userId, DateOnly date)
        {
            try
            {
                var record = new RecordDTO
                {
                    PatientId = patientId,
                    NumberId = numberId,
                    ExamId = examId,
                    UserId = userId,
                    Date = date
                };
                var created = await _recordRepository.AddAsync(record);
                return OperationResult<RecordOutputModel>.Ok(created.Adapt<RecordOutputModel>());
            }
            catch (Exception ex)
            {
                return OperationResult<RecordOutputModel>.Fail(ex.Message);
            }
        }

        public async Task<OperationResult<bool>> DeleteRecordAsync(int recordId)
        {
            try
            {
                var success = await _recordRepository.DeleteAsync(recordId);
                if (!success)
                    return OperationResult<bool>.Fail("Запись не найдена");
                return OperationResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Fail($"Ошибка удаления записи: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<RecordOutputModel>>> GetRecordsByNumberIdAsync(int numberId)
        {
            try
            {
                var records = await _recordRepository.GetByNumberIdAsync(numberId);
                var result = records.Adapt<List<RecordOutputModel>>();
                return OperationResult<List<RecordOutputModel>>.Ok(result);
            }
            catch (Exception ex)
            {
                return OperationResult<List<RecordOutputModel>>.Fail(ex.Message);
            }
        }
    }
}
