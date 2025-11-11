using ADC.Domain.Repositories;
using ADC.Domain.Services;
using ADC.Domain.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;
namespace ADC.Domain;

public static class Extensions
{
    public static IServiceCollection AddDomain(this IServiceCollection service)
    {
        service.AddScoped<IAuthenticationService, AuthenticationService>();
        service.AddScoped<IEncryptService, SHAEncryptService>();
        service.AddSingleton<ITokenService, TokenService>();
        return service;
    }
}