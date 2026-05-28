using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core.DTOs;

namespace XRayJournal.Core.IRepositories
{
    public interface IHospitalRepository
    {
        public Task<HospitalDTO> AddAsync(HospitalDTO hospital);

        public Task<HospitalDTO?> UpdateAsync(HospitalDTO hospital);

        public Task<bool> DeleteAsync(int id);

        public Task<List<HospitalDTO>> GetAllAsync();

        public Task<HospitalDTO?> GetByIdAsync(int id);
    }
}
