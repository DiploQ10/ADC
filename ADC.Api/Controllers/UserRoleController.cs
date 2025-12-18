using ADC.Domain.Responses;
using ADC.Persistence.Models;
using ADC.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ADC.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserRoleController(IUserRoleRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ReadAllResponse<UserRoleEntity>> GetAll()
    {
        return await repository.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ReadOneResponse<UserRoleEntity>> GetById(Guid id)
    {
        return await repository.GetByIdAsync(id);
    }

    [HttpGet("user/{userId}")]
    public async Task<ReadAllResponse<UserRoleEntity>> GetByUser(Guid userId)
    {
        return await repository.GetByUserIdAsync(userId);
    }

    [HttpPost]
    public async Task<CreateResponse> Create([FromBody] UserRoleEntity model)
    {
        return await repository.CreateAsync(model);
    }

    [HttpPut("{id}")]
    public async Task<ResponseBase> Update(Guid id, [FromBody] UserRoleEntity model)
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
