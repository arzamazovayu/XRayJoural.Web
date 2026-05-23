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

        public Task<NumberDTO?> GetLastNumberAsync();

        public Task<List<NumberDTO>> GetAllAsync();

        public Task<List<NumberDTO>> GetByPatientAndExamAsync(int patientId, DateOnly xRayDate);

        public Task<NumberDTO> UpdateAsync(NumberDTO number);

        public bool Delete(int id);
    }
}
