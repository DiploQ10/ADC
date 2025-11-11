using ADC.Domain.Entities;
using ADC.Domain.Repositories;
using ADC.Domain.Responses;
using ADC.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace ADC.Api.Controllers;

[Route("api/[controller]")]
public class UserController(IUserRepository userRepository, IEncryptService encryptService) : ControllerBase
{

    [HttpPost]
    public async Task<CreateResponse> CreateUserAsync([FromBody] UserEntity model)
    {
        model.Password = encryptService.Encrypt(model.Password);
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