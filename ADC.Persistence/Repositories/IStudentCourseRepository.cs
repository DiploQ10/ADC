using ADC.Domain.Responses;
using ADC.Persistence.Models;

namespace ADC.Persistence.Repositories;

public interface IStudentCourseRepository
{
    Task<CreateResponse> CreateAsync(StudentCourseEntity model);
    Task<ReadAllResponse<StudentCourseEntity>> GetAllAsync();
    Task<ReadAllResponse<StudentCourseEntity>> GetByStudentIdAsync(Guid studentId);
    Task<ReadAllResponse<StudentCourseEntity>> GetByCourseIdAsync(Guid courseId);
    Task<ReadOneResponse<StudentCourseEntity>> GetByIdAsync(Guid id);
    Task<ResponseBase> DeleteAsync(Guid id);
    Task<ResponseBase> ExistAsync(Guid id);
}
