using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core.IRepositories;
using XRayJournal.Core.OutputModels;
using XRayJournal.Core.ReportsModels;

namespace XRayJournal.BLL
{
    public class ReportService
    {
        private readonly IRecordRepository _recordRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICabinetRepository _cabinetRepository;

        public ReportService(IRecordRepository recordRepository, IUserRepository userRepository, ICabinetRepository cabinetRepository)
        {
            _recordRepository = recordRepository;
            _userRepository = userRepository;
            _cabinetRepository = cabinetRepository;
        }

        public async Task<List<DailyReportModel>> GetDailyReportAsync(ReportParameters parameters)
        {
            DateOnly start = parameters.StartDate.Value;
            DateOnly end = parameters.EndDate.Value;
            var records = await _recordRepository.GetRecordsForReportAsync(start, end, parameters.CabinetIds, parameters.UserIds);
            Console.WriteLine($"Найдено записей: {records.Count}");

            if (parameters.ByCabinet)
            {
                var grouped = records
                    .Where(r => r.Exam?.Cabinet != null)
                    .GroupBy(r => r.Exam.Cabinet.CabNum)
                    .Select(g => new DailyReportModel
                    {
                        EntityName = g.Key,
                        PatientCount = g.Select(r => r.PatientId).Distinct().Count(),
                        ExamCount = g.Count(),
                        TotalCost = g.Sum(r => r.Exam?.XRayCost ?? 0),
                        Date = start
                    })
                    .ToList();
                return grouped;
            }
            else
            {
                var grouped = records
                    .Where(r => r.User != null)
                    .GroupBy(r => r.User.FIOshort)
                    .Select(g => new DailyReportModel
                    {
                        EntityName = g.Key,
                        PatientCount = g.Select(r => r.PatientId).Distinct().Count(),
                        ExamCount = g.Count(),
                        TotalCost = g.Sum(r => r.Exam?.XRayCost ?? 0),
                        Date = start
                    })
                    .ToList();
                return grouped;
            }
        }

        public async Task<List<WeeklyReportModel>> GetWeeklyReportAsync(ReportParameters parameters)
        {
            DateOnly start = parameters.StartDate.Value;
            DateOnly end = parameters.EndDate.Value;
            var records = await _recordRepository.GetRecordsForReportAsync(start, end, parameters.CabinetIds, parameters.UserIds);
            Console.WriteLine($"Найдено записей: {records.Count}");

            if (parameters.ByCabinet)
            {
                var grouped = records
                    .Where(r => r.Exam?.Cabinet != null)
                    .GroupBy(r => r.Exam.Cabinet.CabNum)
                    .Select(g => new WeeklyReportModel
                    {
                        EntityName = g.Key,
                        PatientCount = g.Select(r => r.PatientId).Distinct().Count(),
                        ExamCount = g.Count(),
                        TotalCost = g.Sum(r => r.Exam?.XRayCost ?? 0),
                        StartDate = start,
                        EndDate = end
                    })
                    .ToList();
                return grouped;
            }
            else
            {
                var grouped = records
                    .Where(r => r.User != null)
                    .GroupBy(r => r.User.FIOshort)
                    .Select(g => new WeeklyReportModel
                    {
                        EntityName = g.Key,
                        PatientCount = g.Select(r => r.PatientId).Distinct().Count(),
                        ExamCount = g.Count(),
                        TotalCost = g.Sum(r => r.Exam?.XRayCost ?? 0),
                        StartDate = start,
                        EndDate = end
                    })
                    .ToList();
                return grouped;
            }
        }
    }
}
