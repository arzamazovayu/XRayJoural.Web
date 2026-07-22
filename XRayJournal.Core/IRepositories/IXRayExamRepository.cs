using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core.DTOs;


namespace XRayJournal.Core.IRepositories
{
    public interface IXRayExamRepository
    {
        public Task<XRayExamDTO> AddAsync(XRayExamDTO exam);

        public Task<bool> UpdateAsync(XRayExamDTO exam);

        public Task<bool> DeleteAsync(int id);

        public Task<XRayExamDTO?> GetByIdAsync(int id);

        public Task<List<XRayExamDTO?>> GetByPatientIdAsync(int patientId);

    }
}
