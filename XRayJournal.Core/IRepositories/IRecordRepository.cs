using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core.DTOs;
using XRayJournal.Core.OutputModels;

namespace XRayJournal.Core.IRepositories
{
    public interface IRecordRepository
    {
        public Task<RecordDTO> AddAsync(RecordDTO record);

        public Task<RecordDTO?> UpdateAsync(RecordDTO record);

        public Task<bool> DeleteAsync(int id);

        public Task<RecordDTO?> GetByIdAsync(int id);

        public Task<List<RecordDTO>> GetAllAsync();

        public Task<List<RecordDTO>> GetByDateRangeAsync(DateOnly start, DateOnly end);

        public Task<List<RecordDTO>> GetByPatientIdAsync(int patientId);

        public Task<List<RecordDTO>> GetByCabinetIdAsync(int cabinetId);

        public Task<List<RecordDTO>> GetByDateAndCabinetAsync(DateOnly date, int cabinetId);

        public Task<List<RecordNecessaryOutputModel>> GetNecessaryRecordsAsync(
            DateOnly? startDate = null, DateOnly? endDate = null, int? cabinetId = null);

        public Task<bool> UpdateExamIdAsync(int recordId, int examId);

        public Task<List<RecordDTO>> GetByNumberIdAsync(int numberId);

    }
}
