using ADC.Domain.Responses;
using ADC.Persistence.Models;
using ADC.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ADC.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentCourseController(IStudentCourseRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ReadAllResponse<StudentCourseEntity>> GetAll()
    {
        return await repository.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ReadOneResponse<StudentCourseEntity>> GetById(Guid id)
    {
        return await repository.GetByIdAsync(id);
    }

    [HttpGet("student/{studentId}")]
    public async Task<ReadAllResponse<StudentCourseEntity>> GetByStudent(Guid studentId)
    {
        return await repository.GetByStudentIdAsync(studentId);
    }

    [HttpGet("course/{courseId}")]
    public async Task<ReadAllResponse<StudentCourseEntity>> GetByCourse(Guid courseId)
    {
        return await repository.GetByCourseIdAsync(courseId);
    }

    [HttpPost]
    public async Task<CreateResponse> Create([FromBody] StudentCourseEntity model)
    {
        return await repository.CreateAsync(model);
    }

    [HttpDelete("{id}")]
    public async Task<ResponseBase> Delete(Guid id)
    {
        return await repository.DeleteAsync(id);
    }
}
