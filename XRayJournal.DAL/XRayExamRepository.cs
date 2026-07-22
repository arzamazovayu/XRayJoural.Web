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
    public class XRayExamRepository : IXRayExamRepository
    {
        private readonly DataContext _dataContext;

        public XRayExamRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<XRayExamDTO> AddAsync(XRayExamDTO exam)
        {
            await _dataContext.Exams.AddAsync(exam);
            await _dataContext.SaveChangesAsync();
            return exam;
        }

        public async Task<XRayExamDTO?> GetByIdAsync(int id)
        {
            return await _dataContext.Exams?.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> UpdateAsync(XRayExamDTO exam) 
        {
            var exist = await _dataContext.Exams.FindAsync(exam.Id);
            if (exist == null) 
            {
                return false;
            }
            _dataContext.Entry(exist).CurrentValues.SetValues(exam);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var exam = await _dataContext.Exams.FindAsync(id);
            if (exam == null)
            {
                return false;
            }
            _dataContext.Exams.Remove(exam);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<XRayExamDTO?>> GetByPatientIdAsync(int patientId)
        {
            var result = await _dataContext.Records
                .Where(r => r.PatientId == patientId && r.Exam != null)
                .Include(r => r.Exam)
                .Select(r => r.Exam)
                .OrderByDescending(r => r.XRayDate)
                .ToListAsync();

            return result ?? new List<XRayExamDTO?>();
        }
    }
}
