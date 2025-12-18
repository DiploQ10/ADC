using ADC.Domain.Responses;
using ADC.Persistence.Models;
using ADC.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ADC.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseController(ICourseRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ReadAllResponse<CourseEntity>> GetAll()
    {
        return await repository.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ReadOneResponse<CourseEntity>> GetById(Guid id)
    {
        return await repository.GetByIdAsync(id);
    }

    [HttpGet("instructor/{instructorId}")]
    public async Task<ReadAllResponse<CourseEntity>> GetByInstructor(Guid instructorId)
    {
        return await repository.GetByInstructorIdAsync(instructorId);
    }

    [HttpPost]
    public async Task<CreateResponse> Create([FromBody] CourseEntity model)
    {
        return await repository.CreateAsync(model);
    }

    [HttpPut("{id}")]
    public async Task<ResponseBase> Update(Guid id, [FromBody] CourseEntity model)
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
