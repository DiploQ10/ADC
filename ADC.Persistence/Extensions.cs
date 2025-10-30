using ADC.Persistence.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace ADC.Persistence;

public static class Extensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection service, IConfiguration configuration)
    {
        // Agregar el contexto de EF Core usando Npgsql
        service.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration["ConnectionStrings:release"]));

        service.AddScoped<Repositories.IUserRepository, Repositories.EF.UserRepository>();
        return service;
    }

    public static IApplicationBuilder UsePersistence(this IApplicationBuilder app)
    {
        var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        bool @is = context.Database.EnsureCreated();

        return app;
    }

}