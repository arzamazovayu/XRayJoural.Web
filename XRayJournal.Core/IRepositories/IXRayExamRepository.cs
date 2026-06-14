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
        public XRayExamDTO Add(XRayExamDTO exam);

        public bool Update(XRayExamDTO exam);

        public bool Delete(int id);

        public XRayExamDTO? GetById(int id);

        public List<XRayExamDTO?> GetByPatientId(int patientId);

    }
}
