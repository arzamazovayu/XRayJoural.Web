using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core.DTOs;
using XRayJournal.Core.OutputModels;

namespace XRayJournal.Core
{
    public class MapsterConfig : IRegister
    {
        public void Register(TypeAdapterConfig config) 
        {
            config.NewConfig<PatientDTO, PatientOutputModel>();
                //.Map(p=>p.SecondName, dto=>dto.SecondName!.ToUpper);

            config.NewConfig<XRayExamDTO, XRayExamOutputModel>();

            config.NewConfig<PatientDTO, PatientWithExamOutputModel>()
                .Map(dest => dest.Exams, src => src.Exams.Adapt<List<XRayExamOutputModel>>());
        }
    }
}
