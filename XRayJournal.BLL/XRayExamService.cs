using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public List<XRayExamOutputModel> GetAllExams()
        {
            var tmp = _xRayExamRepository.GetAllExams();
            var result = tmp.Adapt<List<XRayExamOutputModel>>();
            return result;
        }

        public List<XrayExamNecessaryInfoOutputModel> GetNecessaryExams()
        {
            var tmp = _xRayExamRepository.GetNecessaryExams();
            var result = tmp.Adapt<List<XrayExamNecessaryInfoOutputModel>>();
            return result;
        }

        public OperationResult<XRayExamOutputModel> Add(XRayExamInputModel exam)
        {
            try 
            {
                var examDto = exam.Adapt<XRayExamDTO>();
                var result = _xRayExamRepository.Add(examDto);
                var outputModel = result.Adapt<XRayExamOutputModel>();
                return OperationResult<XRayExamOutputModel>.Ok(outputModel);
            }
            catch (Exception ex)
            {
                return OperationResult<XRayExamOutputModel>.Fail($"Ошибка при добавлении исследования: {ex.Message}");
            }
        }

        public OperationResult<XRayExamOutputModel> Update(XRayExamInputModel exam)
        {
            try
            {
                if (!exam.Id.HasValue)
                {
                    OperationResult<XRayExamOutputModel>.Fail("ID исследования не указан!");
                }
                var examDto = exam.Adapt<XRayExamDTO>();
                examDto.Id = exam.Id.Value;

                var success = _xRayExamRepository.Update(examDto);
                if (!success)
                {
                    return OperationResult<XRayExamOutputModel>.Fail("Исследование не найдено");
                }

                var updatedExam = _xRayExamRepository.GetById(exam.Id.Value);
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

        public OperationResult Delete(int id)
        {
            try
            {
                var success = _xRayExamRepository.Delete(id);
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

        public OperationResult<XRayExamOutputModel> GetById(int id)
        {
            try
            {
                var exam = _xRayExamRepository.GetById(id);
                if (exam == null)
                {
                    return OperationResult<XRayExamOutputModel>.Fail("Исследование не найдено");
                }

                var outputModel = exam.Adapt<XRayExamOutputModel>();
                return OperationResult<XRayExamOutputModel>.Ok(outputModel);
            }
            catch (Exception ex)
            {
                return OperationResult<XRayExamOutputModel>.Fail($"Ошибка при получении исследования: {ex.Message}");
            }
        }
    }
}
