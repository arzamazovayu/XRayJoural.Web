using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core;
using XRayJournal.Core.DTOs;
using XRayJournal.Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace XRayJournal.DAL
{
    public class HospitalRepository : IHospitalRepository
    {
        private readonly DataContext _dataContext;
        public HospitalRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<HospitalDTO> AddAsync(HospitalDTO hospital)
        {
            _dataContext.Hospitals.Add(hospital);
            await _dataContext.SaveChangesAsync();
            return hospital;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var hospital = await _dataContext.Hospitals.FindAsync(id);
            if (hospital == null)
            {
                return false;
            }

            _dataContext.Hospitals.Remove(hospital);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<HospitalDTO>> GetAllAsync()
        {
            var result = await _dataContext.Hospitals.OrderBy(p => p.Id).ToListAsync();
            return result;
        }

        public async Task<HospitalDTO?> GetByIdAsync(int id)
        {
            var result = await _dataContext.Hospitals.SingleAsync(p => p.Id == id);
            return result;
        }

        public async Task<HospitalDTO?> UpdateAsync(HospitalDTO hospital)
        {
            var exist = await _dataContext.Hospitals.FindAsync(hospital.Id);
            if (exist == null)
            {
                return null;
            }

            _dataContext.Entry(exist).CurrentValues.SetValues(hospital);
            await _dataContext.SaveChangesAsync();
            return exist;
        }
    }
}
