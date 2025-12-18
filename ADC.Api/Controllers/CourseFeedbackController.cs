using ADC.Domain.Responses;
using ADC.Persistence.Models;
using ADC.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ADC.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseFeedbackController(ICourseFeedbackRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ReadAllResponse<CourseFeedbackEntity>> GetAll()
    {
        return await repository.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ReadOneResponse<CourseFeedbackEntity>> GetById(Guid id)
    {
        return await repository.GetByIdAsync(id);
    }

    [HttpGet("course/{courseId}")]
    public async Task<ReadAllResponse<CourseFeedbackEntity>> GetByCourse(Guid courseId)
    {
        return await repository.GetByCourseIdAsync(courseId);
    }

    [HttpGet("student/{studentId}")]
    public async Task<ReadAllResponse<CourseFeedbackEntity>> GetByStudent(Guid studentId)
    {
        return await repository.GetByStudentIdAsync(studentId);
    }

    [HttpPost]
    public async Task<CreateResponse> Create([FromBody] CourseFeedbackEntity model)
    {
        return await repository.CreateAsync(model);
    }

    [HttpPut("{id}")]
    public async Task<ResponseBase> Update(Guid id, [FromBody] CourseFeedbackEntity model)
    {
        model.Id = id;
        return await repository.UpdateAsync(model);
    }

    [HttpDelete("{id}")]
    public async Task<ResponseBase> Delete(Guid id)
    {
        return await repository.DeleteAsync(id);
    }
}
