using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core.DTOs;
using XRayJournal.Core.IRepositories;
using XRayJournal.Core.OutputModels;
using XRayJournal.Core.InputModels;
using XRayJournal.Core.Results;
using Mapster;


namespace XRayJournal.BLL
{
    public class CabinetService
    {
        public ICabinetRepository _cabinetRepository;

        public CabinetService(ICabinetRepository cabinetRepository)
        {
            _cabinetRepository = cabinetRepository;
        }

        public async Task<OperationResult<List<CabinetOutputModel>>> GetAllAsync()
        {
            try
            {
                var cabinets = await _cabinetRepository.GetAllAsync();
                var result = cabinets.Adapt<List<CabinetOutputModel>>();
                return OperationResult<List<CabinetOutputModel>>.Ok(result);
            }
            catch (Exception ex)
            {
                return OperationResult<List<CabinetOutputModel>>.Fail($"Ошибка получения списка кабинетов: {ex.Message}");
            }
        }

        public async Task<OperationResult<CabinetOutputModel>> GetByIdAsync(int id)
        {
            try
            {
                var cabinet = await _cabinetRepository.GetByIdAsync(id);
                if (cabinet != null)
                {
                    var result = cabinet.Adapt<CabinetOutputModel>();
                    return OperationResult<CabinetOutputModel>.Ok(result);
                }
                else
                {
                    return OperationResult<CabinetOutputModel>.Fail("Кабинет не найден");
                }
            }
            catch(Exception ex)
            {
                return OperationResult<CabinetOutputModel>.Fail($"Ошибка получения кабинета по id: {ex.Message}");
            }
        }
    }
}
