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
        public DataContext _dataContext;

        public XRayExamRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public List<XRayExamDTO> GetAllExams() 
        { 
            var result = _dataContext.Exams.Include(e => e.Patient).OrderBy(p=>p.Id).ToList();
            return result;
        }

        public List<XRayExamDTO> GetNecessaryExams()
        {
            var result = _dataContext.Exams.Include(e => e.Patient).OrderBy(p => p.Id).ToList();
            return result;
        }

        public XRayExamDTO Add(XRayExamDTO exam)
        {
            _dataContext.Exams.Add(exam);
            _dataContext.SaveChanges();
            return exam;
        }

        public XRayExamDTO GetById(int id)
        {
            return _dataContext.Exams.Include(e => e.Patient).FirstOrDefault(e => e.Id == id);
        }

        public bool Update(XRayExamDTO exam) 
        {
            var exist = _dataContext.Exams.Find(exam.Id);
            if (exist == null) 
            {
                return false;
            }
            _dataContext.Entry(exist).CurrentValues.SetValues(exam);
            _dataContext.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var exam = _dataContext.Exams.Find(id);
            if (exam == null)
            {
                return false;
            }
            _dataContext.Exams.Remove(exam);
            _dataContext.SaveChanges();
            return true;
        }

    }
}
