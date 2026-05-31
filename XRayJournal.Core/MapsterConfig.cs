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

            //config.NewConfig<PatientDTO, PatientWithExamOutputModel>()
            //    .Map(dest => dest.Exams, src => src.Exams.Adapt<List<XRayExamOutputModel>>())
            //    .Map(dest => dest.Numbers, src => src.Numbers.Adapt<List<NumberOutputModel>>());

            config.NewConfig<XRayExamDTO, XRayExamOutputModel>();

            config.NewConfig<XRayExamInputModel, XRayExamDTO>()
                .Map(dest => dest.Id, src => src.Id ?? 0);

            config.NewConfig<XRayExamOutputModel, XRayExamInputModel>()
                .Map(dest => dest.Id, src => src.Id);

            //config.NewConfig<XRayExamDTO, XrayExamNecessaryInfoOutputModel>();

            config.NewConfig<NumberDTO, NumberOutputModel>()
                .Map(dest => dest.YearlyNum, src => src.YearlyNum)
                .Map(dest => dest.DailyNum, src => src.DailyNum);

            config.NewConfig<NumberInputModel, NumberDTO>()
                .Map(dest => dest.YearlyNum, src => src.YearlyNum)
                .Map(dest => dest.DailyNum, src => src.DailyNum);

            config.NewConfig<RecordDTO, RecordOutputModel>()
                .Map(dest => dest.DisplayNumber, src => $"{src.Number.YearlyNum}/{src.Number.DailyNum}")
                .Map(dest => dest.UserFIO, src => src.User.FIO);

            config.NewConfig<CabinetDTO, CabinetOutputModel>();

            config.NewConfig<HospitalDTO, HospitalOutputModel>();

            config.NewConfig<RecordDTO, RecordNecessaryOutputModel>()
                .Map(dest => dest.DisplayNumber, src => $"{src.Number.YearlyNum}/{src.Number.DailyNum}")
                .Map(dest => dest.SecondName, src => src.Patient.SecondName)
                .Map(dest => dest.FirstName, src => src.Patient.FirstName)
                .Map(dest => dest.ThirdName, src => src.Patient.ThirdName)
                .Map(dest => dest.BirthDate, src => src.Patient.BirthDate)
                .Map(dest => dest.MedNumber, src => src.Patient.MedNumber)
                .Map(dest => dest.Category, src => src.Exam.Category)
                .Map(dest => dest.XRayName, src => src.Exam.XRayName)
                .Map(dest => dest.XRayShots, src => src.Exam.XRayShots)
                .Map(dest => dest.XRayDose, src => src.Exam.XRayDose)
                .Map(dest => dest.XRayDate, src => src.Exam.XRayDate)
                .Map(dest => dest.DepName, src => src.Exam.Cabinet.Hospital.DepName)
                .Map(dest => dest.RecordDate, src => src.Date);
        }
    }
}
