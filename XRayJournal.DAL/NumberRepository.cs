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
    public class NumberRepository : INumberRepository
    {
        public DataContext _dataContext;

        public NumberRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<NumberDTO?> AddAsync(NumberDTO number)
        {
            _dataContext.Numbers.Add(number);
            await _dataContext.SaveChangesAsync();
            return number;
        }

        public async Task<NumberDTO?> GetLastNumberAsync()
        {
            return await _dataContext.Numbers.
                OrderByDescending(n => n.Id).
                FirstOrDefaultAsync();
        }

        public async Task<List<NumberDTO>> GetAllAsync()
        {
            return await _dataContext.Numbers.
                OrderBy(n => n.XRayDate).
                ThenBy(n => n.Id).
                ToListAsync();
        }

        public async Task<List<NumberDTO>> GetByPatientAndExamAsync(int patientId, DateOnly xRayDate)
        {
            return await _dataContext.Numbers
                .Where(n => n.PatientId == patientId && n.XRayDate == xRayDate).ToListAsync();
        }
        public async Task<NumberDTO?> UpdateAsync(NumberDTO number)
        {

            var existing = await _dataContext.Numbers.FindAsync(number.Id);
            if (existing == null)
            {
                return null;
            }

            existing.YearlyNum = number.YearlyNum;
            existing.DailyNum = number.DailyNum;
            existing.XRayDate = number.XRayDate;

            await _dataContext.SaveChangesAsync();

            return existing;
        }

        public bool Delete(int id)
        {
            var number = _dataContext.Numbers.Find(id);
            if (number == null)
            {
                return false;
            }
            _dataContext.Numbers.Remove(number);
            _dataContext.SaveChanges();
            return true;
        }
    }
}
