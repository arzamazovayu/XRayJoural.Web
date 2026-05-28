using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core.DTOs;

namespace XRayJournal.Core.IRepositories
{
    public interface ICabinetRepository
    {
        public Task<CabinetDTO> AddAsync(CabinetDTO cabinet);

        public Task<CabinetDTO?> UpdateAsync(CabinetDTO cabinet);

        public Task<bool> DeleteAsync(int id);

        public Task<List<CabinetDTO>> GetAllAsync();

        public Task<CabinetDTO?> GetByIdAsync(int id);
    }
}
