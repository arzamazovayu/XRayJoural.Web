using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core.DTOs;
using XRayJournal.Core.InputModels;
using XRayJournal.Core.OutputModels;

namespace XRayJournal.Core
{
    public class MapsterConfig : IRegister
    {
        public void Register(TypeAdapterConfig config) 
        {
            config.NewConfig<PatientDTO, PatientOutputModel>();

            config.NewConfig<PatientDTO, PatientWithExamOutputModel>()
                .Map(dest => dest.Exams, src => src.Exams.Adapt<List<XRayExamOutputModel>>());

            config.NewConfig<XRayExamDTO, XRayExamOutputModel>();

            config.NewConfig<XRayExamInputModel, XRayExamDTO>()
                .Map(dest => dest.Id, src => src.Id ?? 0);

            config.NewConfig<XRayExamOutputModel, XRayExamInputModel>()
                .Map(dest => dest.Id, src => src.Id);
        }
    }
}
