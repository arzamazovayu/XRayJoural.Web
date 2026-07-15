using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core.IRepositories;
using XRayJournal.Core.OutputModels;
using XRayJournal.Core.Results;

namespace XRayJournal.BLL
{
    public class DepartmentService
    {
        public IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<OperationResult<List<DepartmentOutputModel>>> GetAllAsync()
        {
            try
            {
                var departments = await _departmentRepository.GetAllAsync();
                var result = departments.Adapt<List<DepartmentOutputModel>>();
                return OperationResult<List<DepartmentOutputModel>>.Ok(result);
            }
            catch (Exception ex)
            {
                return OperationResult<List<DepartmentOutputModel>>.Fail($"Ошибка получения списка отделений: {ex.Message}");
            }
        }


    }
}
