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
        Task<DepartmentDTO?> GetByIdAsync(int id);

        Task<DepartmentDTO?> GetByDepNameAsync(string depName);

        Task<List<DepartmentDTO>> GetByHospitalAsync(int hospitalId);

        Task<List<DepartmentDTO>> GetAllAsync();
    }
}
