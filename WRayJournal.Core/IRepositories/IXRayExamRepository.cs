using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core.DTOs;


namespace XRayJournal.Core.IRepositories
{
    public interface IXRayExamRepository
    {
        public List<XRayExamDTO> GetAllExams();
    }
}
