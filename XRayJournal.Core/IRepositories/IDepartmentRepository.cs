using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core.DTOs;

namespace XRayJournal.Core.IRepositories
{
    public interface IDepartmentRepository
    {
        public Task<DepartmentDTO?> GetByIdAsync(int id);

        public Task<DepartmentDTO?> GetByDepNameAsync(string depName);

        public Task<List<DepartmentDTO>> GetByHospitalAsync(int hospitalId);

        public Task<List<DepartmentDTO>> GetAllAsync();
    }
}
