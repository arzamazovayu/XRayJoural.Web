using Mapster;
using Microsoft.EntityFrameworkCore;
using XRayJournal.Core;
using XRayJournal.Core.DTOs;
using XRayJournal.Core.IRepositories;
using XRayJournal.Core.OutputModels;



namespace XRayJournal.DAL
{
    public class RecordRepository : IRecordRepository
    {
        private readonly DataContext _dataContext;
        public RecordRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<RecordDTO> AddAsync(RecordDTO record)
        {
            _dataContext.Records.Add(record);
            await _dataContext.SaveChangesAsync();
            return record;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var record = await _dataContext.Records.FindAsync(id);
            if (record == null)
            {
                return false;
            }

            _dataContext.Records.Remove(record);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<RecordDTO>> GetAllAsync()
        {
            return await _dataContext.Records
                .Include(r => r.Patient)
                .Include(r => r.Number)
                .Include(r => r.Exam)
                .ThenInclude(e => e.Cabinet)
                .Include(r => r.User)
                .OrderBy(r => r.Date)
                .ThenBy(r => r.Number.YearlyNum)
                .ToListAsync();
        }

        public async Task<List<RecordDTO>> GetByCabinetIdAsync(int cabinetId)
        {
            return await _dataContext.Records
                .Include(r => r.Patient)
                .Include(r => r.Number)
                .Include(r => r.Exam)
                .Include(r => r.User)
                .Where(r => r.Exam.IdCabinet == cabinetId)
                .OrderBy(r => r.Date)
                .ToListAsync();
        }

        public async Task<List<RecordDTO>> GetByDateAndCabinetAsync(DateOnly date, int cabinetId)
        {
            return await _dataContext.Records
                .Include(r => r.Patient)
                .Include(r => r.Number)
                .Include(r => r.Exam)
                .Include(r => r.User)
                .Where(r => r.Date == date && r.Exam.IdCabinet == cabinetId)
                .OrderBy(r => r.Number.DailyNum)
                .ToListAsync();
        }

        public async Task<List<RecordDTO>> GetByDateRangeAsync(DateOnly start, DateOnly end)
        {
            return await _dataContext.Records
                .Include(r => r.Patient)
                .Include(r => r.Number)
                .Include(r => r.Exam)
                .ThenInclude(e => e.Cabinet)
                .Include(r => r.User)
                .Where(r => r.Date >= start && r.Date <= end)
                .OrderBy(r => r.Date)
                .ThenBy(r => r.Number.YearlyNum)
                .ToListAsync();
        }

        public async Task<RecordDTO?> GetByIdAsync(int id)
        {
            return await _dataContext.Records
                .Include(r => r.Patient)
                .Include(r => r.Number)
                .Include(r => r.Exam)
                .ThenInclude(e => e.Cabinet)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<RecordDTO>> GetByPatientIdAsync(int patientId)
        {
            return await _dataContext.Records
                .Include(r => r.Patient)
                .Include(r => r.Number)
                .Include(r => r.Exam)
                .Include(r => r.User)
                .Where(r => r.PatientId == patientId)
                .OrderByDescending(r => r.Date)
                .ToListAsync();
        }


        public async Task<RecordDTO?> UpdateAsync(RecordDTO record)
        {
            var exist = await _dataContext.Records.FindAsync(record.Id);
            if (exist == null)
            {
                return null;
            }

            _dataContext.Entry(exist).CurrentValues.SetValues(record);
            await _dataContext.SaveChangesAsync();
            return exist;
        }
        public async Task<List<RecordNecessaryOutputModel>> GetNecessaryRecordsAsync(
            DateOnly? startDate = null, DateOnly? endDate = null, int? cabinetId = null)
        {
            //Сборка таблиц
            var query = _dataContext.Records
                .Include(r => r.Patient)
                .Include(r => r.Number)
                .Include(r => r.Exam)
                    .ThenInclude(e => e.Cabinet)
                        .ThenInclude(c => c.Hospital)
                .AsQueryable();

            // Фильтрация по кабинету
            if (cabinetId.HasValue)
            {
                query = query.Where(r => r.Exam != null && r.Exam.IdCabinet == cabinetId.Value);
            }

            //Фильтрация по дате
            if (startDate.HasValue)
            {
                query = query.Where(r => r.Date >= startDate.Value);
            }            
            if (endDate.HasValue)
            {
                query = query.Where(r => r.Date <= endDate.Value);
            }
            //Сортировка по дате и номеру
            var records = await query
                .OrderBy(r => r.Date)
                .ThenBy(r => r.Number != null ? r.Number.YearlyNum : 0)
                .ToListAsync();

            //Группировка по пациенту и дате для определения множественных исследований
            var grouped = records
                .GroupBy(r => new { r.PatientId, r.Date })
                .ToDictionary(g => g.Key, g => g.Count());

            return records.Select(r => r.Adapt<RecordNecessaryOutputModel>()).ToList();//System.NullReferenceException: "Object reference not set to an instance of an object."

        }

        public async Task<bool> UpdateExamIdAsync(int recordId, int examId)
        {
            var record = await _dataContext.Records.FindAsync(recordId);
            if (record == null)
            {
                return false;
            }

            record.ExamId = examId;
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<RecordDTO>> GetByNumberIdAsync(int numberId)
        {
            return await _dataContext.Records
                .Where(r => r.NumberId == numberId)
                .ToListAsync();
        }

        public async Task<List<RecordDTO>> GetRecordsForReportAsync(DateOnly? startDate, DateOnly? endDate,
            List<int>? cabinetIds, List<int>? userIds)
        {
            var query = _dataContext.Records
                .Include(r => r.Patient)
                .Include(r => r.Exam)
                    .ThenInclude(e => e.Cabinet)
                .Include(r => r.User)
                .AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(r => r.Date >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                query = query.Where(r => r.Date <= endDate.Value);
            }

            // Фильтр по кабинетам (через Exam)
            if (cabinetIds != null && cabinetIds.Any())
            {
                query = query.Where(r => r.Exam != null && cabinetIds.Contains(r.Exam.IdCabinet.Value));
            }
            // Фильтр по сотрудникам (UserId)
            var result = new List<RecordDTO>();

            if (userIds != null && userIds.Any())
            {
                var users = await _dataContext.Users
                    .Where(u => userIds.Contains(u.ID))
                    .Select(u => new { u.Role, u.FIOshort, u.ID })
                    .ToListAsync();

                var doctorShortNames = new List<string>();
                var laborantShortNames = new List<string>();
                var otherUserIds = new List<int>();

                foreach (var u in users)
                {
                    if (u.Role == UserRole.Doctor || u.Role == UserRole.Head)
                    {
                        doctorShortNames.Add(u.FIOshort);
                    }
                    else if (u.Role == UserRole.Laborant)
                    {
                        laborantShortNames.Add(u.FIOshort);
                    }
                    else
                    {
                        otherUserIds.Add(u.ID);
                    }
                }

                doctorShortNames = doctorShortNames.Distinct().ToList();
                laborantShortNames = laborantShortNames.Distinct().ToList();

                if (doctorShortNames.Any())
                { 
                    result.AddRange(await query.Where(r => r.Exam != null && doctorShortNames.Contains(r.Exam.Doctor)).ToListAsync()); 
                }
                if (laborantShortNames.Any())
                { 
                    result.AddRange(await query.Where(r => r.Exam != null && laborantShortNames.Contains(r.Exam.Laborant)).ToListAsync()); 
                }
                if (otherUserIds.Any())
                { 
                    result.AddRange(await query.Where(r => otherUserIds.Contains(r.UserId)).ToListAsync()); 
                }
            }
            else
            {
                result = await query.ToListAsync();
            }

            return await query.ToListAsync();
        }
    }
}
