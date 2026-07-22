using System.Text;
using XRayJournal.Core;
using XRayJournal.Core.IRepositories;
using XRayJournal.Core.ReportsModels;

namespace XRayJournal.BLL
{
    public class ReportService
    {
        private readonly IRecordRepository _recordRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICabinetRepository _cabinetRepository;
        private readonly IXRayExamRepository _xRayExamRepository;

        public ReportService(IRecordRepository recordRepository, IUserRepository userRepository, ICabinetRepository cabinetRepository, IXRayExamRepository xRayExamRepository)
        {
            _recordRepository = recordRepository;
            _userRepository = userRepository;
            _cabinetRepository = cabinetRepository;
            _xRayExamRepository = xRayExamRepository;
        }

        public async Task<List<DailyReportModel>> GetDailyReportAsync(ReportParameters parameters)
        {
            DateOnly start = parameters.StartDate.Value;
            DateOnly end = parameters.EndDate.Value;
            var records = await _recordRepository.GetRecordsForReportAsync(start, end, parameters.CabinetIds, parameters.UserIds);

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
                var users = await _userRepository.GetUsersByIdsAsync(parameters.UserIds);
                var result = new List<DailyReportModel>();

                foreach (var user in users)
                {
                    var filtered = records.Where(r => r.Exam != null &&
                        (user.Role == UserRole.Doctor || user.Role == UserRole.Head) && r.Exam.Doctor == user.FIOshort ||
                        (user.Role == UserRole.Laborant) && r.Exam.Laborant == user.FIOshort)
                        .ToList();
                    if (filtered.Any())
                    {
                        result.Add(new DailyReportModel
                        {
                            EntityName = user.FIOshort,
                            PatientCount = filtered.Select(r => r.PatientId).Distinct().Count(),
                            ExamCount = filtered.Count(),
                            TotalCost = filtered.Sum(r => r.Exam?.XRayCost ?? 0),
                            Date = start
                        });
                    }
                }
                return result;
            }
        }

        public async Task<List<WeeklyReportModel>> GetWeeklyReportAsync(ReportParameters parameters)
        {
            DateOnly start = parameters.StartDate.Value;
            DateOnly end = parameters.EndDate.Value;
            var records = await _recordRepository.GetRecordsForReportAsync(start, end, parameters.CabinetIds, parameters.UserIds);

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
                var users = await _userRepository.GetUsersByIdsAsync(parameters.UserIds);
                var result = new List<WeeklyReportModel>();

                foreach (var user in users)
                {
                    var filtered = records.Where(r => r.Exam != null &&
                        (user.Role == UserRole.Doctor || user.Role == UserRole.Head) && r.Exam.Doctor == user.FIOshort ||
                        (user.Role == UserRole.Laborant) && r.Exam.Laborant == user.FIOshort)
                        .ToList();
                    if (filtered.Any())
                    {
                        result.Add(new WeeklyReportModel
                        {
                            EntityName = user.FIOshort,
                            PatientCount = filtered.Select(r => r.PatientId).Distinct().Count(),
                            ExamCount = filtered.Count(),
                            TotalCost = filtered.Sum(r => r.Exam?.XRayCost ?? 0),
                            StartDate = start,
                            EndDate = end
                        });
                    }
                }
                return result;
            }
        }

        public async Task<List<RadiationReportModel>> GetRadiationReportAsync(int patientId, DateOnly startDate, DateOnly endDate)
        {
            var exams = await _xRayExamRepository.GetByPatientIdAsync(patientId); // Получение данных
            var filtered = exams
                .Where(e => e.XRayDate >= startDate && e.XRayDate <= endDate)    // Фильтрация
                .OrderBy(e => e.XRayDate)   // Упорядочивание
                .ToList();

            int counter = 1;
            var result = filtered
                .Select(e => new RadiationReportModel // Сборка модели данных
                {
                    Counter = counter++,
                    Date = e.XRayDate,
                    ExamName = e.XRayName,
                    Dose = e.XRayDose,
                    Note = string.Empty
                })
                .ToList();

            return result;
        }

        public async Task<List<YearlyReportModel>> GetYearlyReportAsync(ReportParameters parameters, DateOnly start, DateOnly end)
        {
            var records = await _recordRepository.GetRecordsForReportAsync(start, end, parameters.CabinetIds, null);
            // Группируем по кабинетам, а внутри – по модальности и области
            var result = new List<YearlyReportModel>();
            foreach (var record in records.Where(r => r.Exam != null))
            {
                var exam = record.Exam;
                result.Add(new YearlyReportModel
                {
                    CabinetNum = exam.Cabinet?.CabNum ?? "Не указан",
                    Modality = exam.XRayModality ?? "Не указана",
                    AreaCode = exam.Area,
                    AreaName = GetAreaName(exam.Area),
                    Count = 1
                });
            }
            // Суммируем Count по группам (CabinetNum, Modality, Area)
            var grouped = result
                .GroupBy(r => new { r.CabinetNum, r.Modality, r.AreaCode, r.AreaName })
                .Select(g => new YearlyReportModel
                {
                    CabinetNum = g.Key.CabinetNum,
                    Modality = g.Key.Modality,
                    AreaCode = g.Key.AreaCode,
                    AreaName = g.Key.AreaName,
                    Count = g.Sum(x => x.Count)
                })
                .ToList();
            return grouped;
        }

        private string GetAreaName(int areaCode)
        {
            return areaCode switch
            {
                10 => "Голова",
                20 => "Шея",
                30 => "Грудь",
                40 => "Живот",
                50 => "Конечности",
                60 => "Таз",
                70 => "Позвоночник",
                80 => "Плод",
                90 => "Всё тело",
                0 => "Прочее"
            };
        }

        public async Task<List<JournalReportModel>> GetJournalReportAsync(int cabinetId, DateOnly startDate, DateOnly endDate)
        {
            var records = await _recordRepository.GetRecordsForReportAsync(startDate, endDate, new List<int> { cabinetId }, null);

            var result = new List<JournalReportModel>();

            var groupedByDate = records
                .OrderBy(r => r.Date)
                .ThenBy(r => r.Number.YearlyNum)
                .GroupBy(r => r.Date);

            foreach (var dateGroup in groupedByDate)
            {
                var dayData = new JournalReportModel { Date = dateGroup.Key };

                var patientsInDay = new List<JournalPatientGroup>();

                var summary = new JournalDaySummary();

                var uniquePatients = dateGroup.Select(r => r.PatientId).Distinct().ToList();

                summary.PatientCount = uniquePatients.Count;

                // Группируем по пациенту внутри дня
                var patientGroups = dateGroup.GroupBy(r => r.PatientId);

                foreach (var patientGroup in patientGroups)
                {
                    var firstRecord = patientGroup.First();
                    if (firstRecord == null)
                    {
                        continue;
                    }

                    var patientGroupModel = new JournalPatientGroup
                    {
                        DisplayNumber = $"{firstRecord.Number.YearlyNum}/{firstRecord.Number.DailyNum}",
                        PatientFIO = $"{firstRecord.Patient.SecondName} {firstRecord.Patient.FirstName} {firstRecord.Patient.ThirdName}".Trim(),
                        BirthDate = firstRecord.Patient.BirthDate.ToString("dd.MM.yyyy"),
                        MedNumber = firstRecord.Patient.MedNumber,
                        Category = firstRecord.Exam?.Category ?? "",
                        DepName = firstRecord.Exam?.Department?.DepName ?? "н/у", //теперь из department
                        Exams = new List<JournalExamItem>()
                    };


                    foreach (var record in patientGroup.OrderBy(r => r.Exam?.XRayDate))
                    {
                        if (record.Exam != null)
                        {
                            patientGroupModel.Exams.Add(new JournalExamItem
                            {
                                ExamName = record.Exam.XRayName,
                                Shots = record.Exam.XRayShots,
                                Dose = record.Exam.XRayDose
                            });
                            // Подсчёт для summary
                            summary.TotalExams++;

                            summary.TotalShots += record.Exam.XRayShots;

                            if (!string.IsNullOrEmpty(record.Exam.Doctor))
                            {
                                summary.Doctor = record.Exam.Doctor;
                            }

                            if (!string.IsNullOrEmpty(record.Exam.Laborant))
                            {
                                summary.Laborant = record.Exam.Laborant;
                            }

                            if (!summary.CategoryCounts.TryGetValue(record.Exam.Category, out int value))
                            {
                                value = 0;
                                summary.CategoryCounts[record.Exam.Category] = value;
                            }

                            summary.CategoryCounts[record.Exam.Category] = ++value;
                        }
                    }
                    patientsInDay.Add(patientGroupModel);
                }
                dayData.PatientGroups = patientsInDay;

                dayData.Summary = summary;

                result.Add(dayData);
            }
            return result;
        }

        public string BuildYearlyCsv(List<YearlyReportModel> data)
        {
            // Собираем уникальные модальности и области
            var modalities = data.Select(d => d.Modality).Distinct().OrderBy(m => m).ToList();
            var areas = data.Select(d => d.AreaName).Distinct().OrderBy(a => a).ToList();

            var sb = new StringBuilder();
            // Формируем заголовок
            sb.Append("Кабинет;Область");
            foreach (var mod in modalities)
            {
                sb.Append($";{mod}"); // Модальности подряд
            }
            sb.AppendLine(";Итого");

            // Группируем по кабинетам и областям
            var grouped = data.GroupBy(d => new { d.CabinetNum, d.AreaName })
                              .Select(g => new { g.Key.CabinetNum, g.Key.AreaName, Items = g.ToList() })
                              .OrderBy(g => g.CabinetNum).ThenBy(g => g.AreaName);

            foreach (var group in grouped)
            {
                // Инициализация нулевого словаря
                var row = new Dictionary<string, int>();
                foreach (var mod in modalities)
                {
                    row[mod] = 0;
                }

                // Подсчитываем суммарное значение в каждой группе
                int total = 0;
                foreach (var item in group.Items)
                {
                    row[item.Modality] += item.Count;
                    total += item.Count;
                }

                // Сложение .csv строки
                sb.Append($"{group.CabinetNum};{group.AreaName}");
                foreach (var mod in modalities)
                {
                    sb.Append($";{row[mod]}");
                }
                sb.AppendLine($";{total}");
            }

            // Строка "Итого по всем кабинетам"
            sb.Append("ИТОГО;");

            var totalsByModality = modalities.ToDictionary(m => m, m => data.Where(d => d.Modality == m).Sum(d => d.Count));

            foreach (var mod in modalities)
            {
                sb.Append($";{totalsByModality[mod]}");
            }
            sb.AppendLine($";{data.Sum(d => d.Count)}");

            return sb.ToString();
        }

        public async Task<string> GetYearlyReportCsvAsync(ReportParameters parameters, DateOnly start, DateOnly end)
        {
            var data = await GetYearlyReportAsync(parameters, start, end);
            return BuildYearlyCsv(data);
        }
    }
}
