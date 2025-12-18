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

        // Registrar todos los repositorios
        service.AddScoped<Repositories.IUserRepository, Repositories.EF.UserRepository>();
        service.AddScoped<Repositories.ICourseRepository, Repositories.EF.CourseRepository>();
        service.AddScoped<Repositories.ISectionRepository, Repositories.EF.SectionRepository>();
        service.AddScoped<Repositories.ILessonRepository, Repositories.EF.LessonRepository>();
        service.AddScoped<Repositories.IUserRoleRepository, Repositories.EF.UserRoleRepository>();
        service.AddScoped<Repositories.IStudentCourseRepository, Repositories.EF.StudentCourseRepository>();
        service.AddScoped<Repositories.ICourseFeedbackRepository, Repositories.EF.CourseFeedbackRepository>();
        
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