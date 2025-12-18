using ADC.Domain.Responses;
using ADC.Persistence.Models;

namespace ADC.Persistence.Repositories;

public interface ISectionRepository
{
    Task<CreateResponse> CreateAsync(SectionEntity model);
    Task<ReadAllResponse<SectionEntity>> GetAllAsync();
    Task<ReadAllResponse<SectionEntity>> GetByCourseIdAsync(Guid courseId);
    Task<ReadOneResponse<SectionEntity>> GetByIdAsync(Guid id);
    Task<ResponseBase> UpdateAsync(SectionEntity model);
    Task<ResponseBase> DeleteAsync(Guid id);
    Task<ResponseBase> ExistAsync(Guid id);
}
