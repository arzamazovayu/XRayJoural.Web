using Mapster;
using XRayJournal.Core.DTOs;
using XRayJournal.Core.InputModels;
using XRayJournal.Core.IRepositories;
using XRayJournal.Core.OutputModels;
using XRayJournal.Core.Results;


namespace XRayJournal.BLL
{
    public class XRayExamService
    {
        private readonly IXRayExamRepository _xRayExamRepository;

        public XRayExamService(IXRayExamRepository xRayExamRepository)
        {
            _xRayExamRepository = xRayExamRepository;
        }

        public async Task<OperationResult<XRayExamOutputModel>> AddAsync(XRayExamInputModel exam)
        {
            try
            {
                var examDto = exam.Adapt<XRayExamDTO>();
                var result = await _xRayExamRepository.AddAsync(examDto);
                var outputModel = result.Adapt<XRayExamOutputModel>();
                return OperationResult<XRayExamOutputModel>.Ok(outputModel);
            }
            catch (Exception ex)
            {
                return OperationResult<XRayExamOutputModel>.Fail($"Ошибка при добавлении исследования: {ex.Message}");
            }
        }

        public async Task<OperationResult<XRayExamOutputModel>> UpdateAsync(XRayExamInputModel exam)
        {
            try
            {
                var examDto = exam.Adapt<XRayExamDTO>();
                if (exam.Id.HasValue)
                {
                    examDto.Id = exam.Id.Value;
                }
                else
                {
                    OperationResult<XRayExamOutputModel>.Fail("ID исследования не указан!");
                }

                var success = await _xRayExamRepository.UpdateAsync(examDto);
                if (!success)
                {
                    return OperationResult<XRayExamOutputModel>.Fail("Исследование не найдено");
                }

                var updatedExam = await _xRayExamRepository.GetByIdAsync(exam.Id.Value);
                if (updatedExam == null)
                {
                    return OperationResult<XRayExamOutputModel>.Fail("Не удалось загрузить обновлённое исследование");
                }
                var outputModel = updatedExam.Adapt<XRayExamOutputModel>();
                return OperationResult<XRayExamOutputModel>.Ok(outputModel);
            }
            catch (Exception ex)
            {
                return OperationResult<XRayExamOutputModel>.Fail($"Ошибка при обновлении исследования: {ex.Message}");
            }
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                var success = await _xRayExamRepository.DeleteAsync(id);
                if (!success)
                {
                    return OperationResult.Fail("Исследование не найдено");
                }
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                return OperationResult.Fail($"Ошибка при удалении исследования: {ex.Message}");
            }
        }

    }
}
