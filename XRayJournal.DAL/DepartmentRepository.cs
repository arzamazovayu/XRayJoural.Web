using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core;
using XRayJournal.Core.DTOs;
using XRayJournal.Core.IRepositories;

namespace XRayJournal.DAL
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DataContext _dataContext;
        public DepartmentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<DepartmentDTO?> GetByDepNameAsync(string depName)
        {
            return await _dataContext.Departments.Where(d => d.DepName == depName).SingleAsync();
        }

        public async Task<List<DepartmentDTO>> GetByHospitalAsync(int hospitalId)
        {
            return await _dataContext.Departments
                .Where(d => d.HospitalId == hospitalId)
                .ToListAsync();
        }

        public async Task<DepartmentDTO?> GetByIdAsync(int id)
        {
            var result = await _dataContext.Departments.SingleAsync(p => p.Id == id);
            return result;
        }

        public async Task<List<DepartmentDTO>> GetAllAsync()
        {
            var result = await _dataContext.Departments.OrderBy(d => d.Id).ToListAsync();
            return result;
        }
    }
}
