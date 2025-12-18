using ADC.Domain.DTOs;

namespace ADC.Infraestructure.Interfaces;

public interface IAuthService
{
    Task<string> GenerateTokenAsync(string userId, string email, string role);
    Task<string> HashPasswordAsync(string password);
    Task<bool> VerifyPasswordAsync(string password, string hash);
    Task<AuthResponse> RegisterAsync(UserRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}
