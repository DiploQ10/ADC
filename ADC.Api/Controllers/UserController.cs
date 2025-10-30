using ADC.Domain.Responses;
using ADC.Persistence.Models;
using Microsoft.AspNetCore.Mvc;

namespace ADC.Api.Controllers;

[Route("api/[controller]")]
public class UserController(ADC.Persistence.Repositories.IUserRepository userRepository) : ControllerBase
{

    [HttpPost]
    public async Task<CreateResponse> CreateUserAsync([FromBody] Persistence.Models.UserEntity model)
    {
        var response = await userRepository.CreateAsync(model);
        return response;
    }

    [HttpGet]
    public async Task<ReadAllResponse<UserEntity>> GetAll()
    {
        var response = await userRepository.GetAllAsync();
        return response;
    }

}