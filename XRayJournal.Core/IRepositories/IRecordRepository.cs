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

        public Task<bool> DeleteAsync(int id);

        public Task<RecordDTO?> GetByIdAsync(int id);

        public Task<List<RecordNecessaryOutputModel>> GetNecessaryRecordsAsync(
            DateOnly? startDate = null, DateOnly? endDate = null, int? cabinetId = null);

        public Task<List<RecordDTO>> GetByNumberIdAsync(int numberId);

        public Task<List<RecordDTO>> GetRecordsForReportAsync(DateOnly? startDate, DateOnly? endDate, List<int>? cabinetIds, List<int>? userIds);
    }
}
