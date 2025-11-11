using ADC.Domain.Entities;
using ADC.Domain.Models;
using ADC.Domain.Responses;

namespace ADC.Domain.Services;

public interface IAuthenticationService
{
    string GenerateToken(string user, string role);
    AuthenticationModel Authenticate(string token);
    Task<ReadOneResponse<UserEntity>> Authenticate(string user, string password);
}