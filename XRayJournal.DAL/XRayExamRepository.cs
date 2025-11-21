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
            var result = _dataContext.Exams.OrderBy(p=>p.Id).ToList();
            return result;
        }
    }
}
