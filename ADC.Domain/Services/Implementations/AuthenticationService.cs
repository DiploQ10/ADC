using ADC.Domain.Entities;
using ADC.Domain.Models;
using ADC.Domain.Responses;

namespace ADC.Domain.Services.Implementations;

internal class AuthenticationService(Repositories.IUserRepository userRepository, IEncryptService encryptService, ITokenService tokenService) : IAuthenticationService
{
    public AuthenticationModel Authenticate(string token)
    {
        throw new NotImplementedException();
    }

    public async Task<ReadOneResponse<UserEntity>> Authenticate(string user, string password)
    {
        // Validar datos de entrada.
        if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password))
            return new (Responses.Responses.InvalidParameters);



        var userModel = await userRepository.GetByEmail(user);

        if (userModel.Response is not Responses.Responses.Success)
            return new (Responses.Responses.Unauthorized);

        string encryptedPassword = encryptService.Encrypt(password);

        if (userModel.Model.Password != encryptedPassword)
            return new (Responses.Responses.Unauthorized);

        return new(Responses.Responses.Success, userModel.Model);
    }

    public string GenerateToken(string user, string role)
    {
       return tokenService.Generate(user, 0);
    }
}