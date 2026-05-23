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
                .Map(dest => dest.Exams, src => src.Exams.Adapt<List<XRayExamOutputModel>>())
                .Map(dest => dest.Numbers, src => src.Numbers.Adapt<List<NumberOutputModel>>());

            config.NewConfig<XRayExamDTO, XRayExamOutputModel>();

            config.NewConfig<XRayExamInputModel, XRayExamDTO>()
                .Map(dest => dest.Id, src => src.Id ?? 0);

            config.NewConfig<XRayExamOutputModel, XRayExamInputModel>()
                .Map(dest => dest.Id, src => src.Id);

            config.NewConfig<XRayExamDTO, XrayExamNecessaryInfoOutputModel>();

            config.NewConfig<NumberDTO, NumberOutputModel>()
                .Map(dest => dest.YearlyNum, src => src.YearlyNum)
                .Map(dest => dest.DailyNum, src => src.DailyNum);

            config.NewConfig<NumberInputModel, NumberDTO>()
                .Map(dest => dest.YearlyNum, src => src.YearlyNum)
                .Map(dest => dest.DailyNum, src => src.DailyNum);
        }
    }
}
