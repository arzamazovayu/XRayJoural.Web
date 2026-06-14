using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core.DTOs;
using XRayJournal.Core.IRepositories;
using XRayJournal.Core.Results;
using XRayJournal.Core.OutputModels;
using XRayJournal.Core.InputModels;
using Mapster;

namespace XRayJournal.BLL
{
    public class NumberService
    {
        public INumberRepository _numberRepository;

        public NumberService(INumberRepository numberRepository)
        {
            _numberRepository = numberRepository;
        }

        public async Task<(int Yearly, int Daily)> CalculateNextNumberAsync(DateOnly XRayDate, int cabinetId)
        {
            var lastNum = await _numberRepository.GetLastNumberForCabinetAsync(cabinetId);

            //Если последнего номера нет, то предлагаем 1/1 как первый
            if (lastNum == null) 
            {
                return (1, 1);
            }

            int yearlyNum = lastNum.YearlyNum;
            int dailyNum = lastNum.DailyNum;

            //проверяем отчётные период вводимой записи
            var periodStart = new DateOnly(XRayDate.Year, 12, 1);
            if (XRayDate < periodStart)
            {
                periodStart = new DateOnly(XRayDate.Year - 1, 12, 1);
            }

            //проверяем отчётный период предыдущей записи
            var lastPeriodStart = new DateOnly(lastNum.XRayDate.Year, 12, 1);
            if (lastNum.XRayDate < lastPeriodStart)
            {
                lastPeriodStart = new DateOnly(lastNum.XRayDate.Year - 1, 12, 1);
            }

            //если периоды отличаются, то сбрасываем счётчики
            if (periodStart > lastPeriodStart)
            {
                yearlyNum = 1;
                dailyNum = 1;
            }
            //если дата та же, то увеличиваем счётчики
            else if (XRayDate == lastNum.XRayDate) 
            {
                yearlyNum = lastNum.YearlyNum + 1;
                dailyNum = lastNum.DailyNum + 1;
            }
            //если дата больше, то увеличиваем годовой и сбрасываем дневной счётчики
            else
            {
                yearlyNum = lastNum.YearlyNum + 1;
                dailyNum = 1;
            }

            return (yearlyNum, dailyNum);
        }

        public async Task<OperationResult<NumberOutputModel>> CreateNumberAsync(
            NumberInputModel numberInput, int patientId, DateOnly xRayDate)
        {
            try
            {
                var numberDto = numberInput.Adapt<NumberDTO>();
                numberDto.PatientId = patientId;
                numberDto.XRayDate = xRayDate;

                var created = await _numberRepository.AddAsync(numberDto);
                var result = created.Adapt<NumberOutputModel>();

                return OperationResult<NumberOutputModel>.Ok(result);
            }
            catch (Exception ex)
            {
                return OperationResult<NumberOutputModel>.Fail($"Ошибка создания номера: {ex.Message}");
            }
        }

        public async Task<OperationResult<NumberOutputModel>> UpdateNumberAsync(NumberInputModel number)
        {
            try
            {
                if (number.Id == 0)
                {
                    return OperationResult<NumberOutputModel>.Fail("Id номера не указан");
                }

                var numberDto = number.Adapt<NumberDTO>();
                var updatedNumber = await _numberRepository.UpdateAsync(numberDto);

                if (updatedNumber == null)
                {
                    return OperationResult<NumberOutputModel>.Fail("Номер не найден");
                }

                var outputModel = updatedNumber.Adapt<NumberOutputModel>();
                return OperationResult<NumberOutputModel>.Ok(outputModel);
            }
            catch(Exception ex)
            {
                return OperationResult<NumberOutputModel>.Fail($"Ошибка при добавлении номера: {ex.Message}");
            }
        }

        public async Task<OperationResult<bool>> DeleteNumberAsync(int numberId)
        {
            try
            {
                var success = _numberRepository.Delete(numberId);
                return success ? OperationResult<bool>.Ok(true) : OperationResult<bool>.Fail("Номер не найден");
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Fail($"Ошибка удаления номера: {ex.Message}");
            }
        }
    }
}
