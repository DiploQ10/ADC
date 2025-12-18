using ADC.Domain.Responses;
using ADC.Persistence.Models;

namespace ADC.Persistence.Repositories;

public interface ICourseRepository
{
    Task<CreateResponse> CreateAsync(CourseEntity model);
    Task<ReadAllResponse<CourseEntity>> GetAllAsync();
    Task<ReadOneResponse<CourseEntity>> GetByIdAsync(Guid id);
    Task<ReadAllResponse<CourseEntity>> GetByInstructorIdAsync(Guid instructorId);
    Task<ResponseBase> UpdateAsync(CourseEntity model);
    Task<ResponseBase> DeleteAsync(Guid id);
    Task<ResponseBase> ExistAsync(Guid id);
}