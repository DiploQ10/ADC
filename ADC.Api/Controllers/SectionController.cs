using ADC.Domain.Responses;
using ADC.Persistence.Models;
using ADC.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ADC.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SectionController(ISectionRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ReadAllResponse<SectionEntity>> GetAll()
    {
        return await repository.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ReadOneResponse<SectionEntity>> GetById(Guid id)
    {
        return await repository.GetByIdAsync(id);
    }

    [HttpGet("course/{courseId}")]
    public async Task<ReadAllResponse<SectionEntity>> GetByCourse(Guid courseId)
    {
        return await repository.GetByCourseIdAsync(courseId);
    }

    [HttpPost]
    public async Task<CreateResponse> Create([FromBody] SectionEntity model)
    {
        return await repository.CreateAsync(model);
    }

    [HttpPut("{id}")]
    public async Task<ResponseBase> Update(Guid id, [FromBody] SectionEntity model)
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
