using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XRayJournal.Core.DTOs;

namespace XRayJournal.Core.IRepositories
{
    public interface INumberRepository
    {
        public Task<NumberDTO> AddAsync(NumberDTO number);

        public Task<NumberDTO?> GetLastNumberForCabinetAsync(int cabinetId);

        public Task<NumberDTO> UpdateAsync(NumberDTO number);

        public bool Delete(int id);

        public Task<NumberDTO?> GetByIdAsync(int id);
    }
}
