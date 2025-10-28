using Microsoft.Extensions.DependencyInjection;

namespace ADC.Persistence;

public static class Extensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection service)
    {
        return service;
    }
}