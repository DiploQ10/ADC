using ADC.Domain.Responses;
using ADC.Persistence.Models;

namespace ADC.Persistence.Repositories;

public interface ICourseFeedbackRepository
{
    Task<CreateResponse> CreateAsync(CourseFeedbackEntity model);
    Task<ReadAllResponse<CourseFeedbackEntity>> GetAllAsync();
    Task<ReadAllResponse<CourseFeedbackEntity>> GetByCourseIdAsync(Guid courseId);
    Task<ReadAllResponse<CourseFeedbackEntity>> GetByStudentIdAsync(Guid studentId);
    Task<ReadOneResponse<CourseFeedbackEntity>> GetByIdAsync(Guid id);
    Task<ResponseBase> UpdateAsync(CourseFeedbackEntity model);
    Task<ResponseBase> DeleteAsync(Guid id);
    Task<ResponseBase> ExistAsync(Guid id);
}
