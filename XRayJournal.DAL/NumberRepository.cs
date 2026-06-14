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

        public async Task<NumberDTO> UpdateAsync(NumberDTO number)
        {
            var existing = await _dataContext.Numbers.FindAsync(number.Id);
            if (existing == null) 
            { 
                throw new Exception($"Номер с ID {number.Id} не найден");
            }

            existing.YearlyNum = number.YearlyNum;
            existing.DailyNum = number.DailyNum;
            //Изменять дату не зачем т.к. пользователь не может её редактировать напрямую
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
            return await _dataContext.Records
                        .Where(r => r.Exam != null && r.Exam.IdCabinet == cabinetId)
                        .OrderByDescending(r => r.Number.Id)
                        .Select(r => r.Number)
                        .FirstOrDefaultAsync();
        }

        public async Task<NumberDTO?> GetByIdAsync(int id)
        {
            return await _dataContext.Numbers.FindAsync(id);
        }
    }
}
