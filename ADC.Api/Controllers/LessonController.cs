using ADC.Domain.Responses;
using ADC.Persistence.Models;
using ADC.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ADC.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LessonController(ILessonRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ReadAllResponse<LessonEntity>> GetAll()
    {
        return await repository.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ReadOneResponse<LessonEntity>> GetById(Guid id)
    {
        return await repository.GetByIdAsync(id);
    }

    [HttpGet("section/{sectionId}")]
    public async Task<ReadAllResponse<LessonEntity>> GetBySection(Guid sectionId)
    {
        return await repository.GetBySectionIdAsync(sectionId);
    }

    [HttpPost]
    public async Task<CreateResponse> Create([FromBody] LessonEntity model)
    {
        return await repository.CreateAsync(model);
    }

    [HttpPut("{id}")]
    public async Task<ResponseBase> Update(Guid id, [FromBody] LessonEntity model)
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
