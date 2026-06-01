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
        private readonly DataContext _dataContext;

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
        public async Task<NumberDTO> UpdateAsync(NumberDTO number)
        {
            _dataContext.Numbers.Update(number);
            await _dataContext.SaveChangesAsync();
            return number;
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

        public async Task<NumberDTO?> GetLastNumberForCabinetAsync(int cabinetId)
        {
            var query = from record in _dataContext.Records
                        join exam in _dataContext.Exams on record.ExamId equals exam.Id
                        join number in _dataContext.Numbers on record.NumberId equals number.Id
                        where exam.IdCabinet == cabinetId
                        orderby number.Id descending
                        select number;

            return await query.FirstOrDefaultAsync();
        }

        public async Task<NumberDTO?> GetByIdAsync(int id)
        {
            return await _dataContext.Numbers.FindAsync(id);
        }
    }
}
