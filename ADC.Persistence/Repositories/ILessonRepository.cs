using ADC.Domain.Responses;
using ADC.Persistence.Models;

namespace ADC.Persistence.Repositories;

public interface ILessonRepository
{
    Task<CreateResponse> CreateAsync(LessonEntity model);
    Task<ReadAllResponse<LessonEntity>> GetAllAsync();
    Task<ReadAllResponse<LessonEntity>> GetBySectionIdAsync(Guid sectionId);
    Task<ReadOneResponse<LessonEntity>> GetByIdAsync(Guid id);
    Task<ResponseBase> UpdateAsync(LessonEntity model);
    Task<ResponseBase> DeleteAsync(Guid id);
    Task<ResponseBase> ExistAsync(Guid id);
}
