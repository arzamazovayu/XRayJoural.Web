using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core.IRepositories;
using XRayJournal.Core.OutputModels;


namespace XRayJournal.BLL
{
    public class XRayExamService
    {
        public IXRayExamRepository _xRayExamRepository;

        public XRayExamService(IXRayExamRepository xRayExamRepository)
        {
            _xRayExamRepository = xRayExamRepository;
        }
        public List<XRayExamOutputModel> GetAllExams()
        {
            var tmp = _xRayExamRepository.GetAllExams();
            var result = tmp.Adapt<List<XRayExamOutputModel>>();
            return result;
        }
    }
}
