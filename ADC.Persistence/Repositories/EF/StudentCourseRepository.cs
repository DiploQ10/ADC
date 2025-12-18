using ADC.Domain.Responses;
using ADC.Persistence.Data;
using ADC.Persistence.Models;
using EntityState = ADC.Persistence.Models.Enums.EntityState;
using Microsoft.EntityFrameworkCore;

namespace ADC.Persistence.Repositories.EF;

internal class StudentCourseRepository(DataContext context) : IStudentCourseRepository
{
    public async Task<CreateResponse> CreateAsync(StudentCourseEntity model)
    {
        model.Id = Guid.CreateVersion7();
        model.CreatedAt = DateTime.UtcNow;
        model.State = EntityState.Active;
        
        context.StudentCourses.Add(model);
        await context.SaveChangesAsync();
        
        return new CreateResponse(Responses.Success, model.Id, "Estudiante inscrito en curso exitosamente");
    }

    public async Task<ReadAllResponse<StudentCourseEntity>> GetAllAsync()
    {
        var studentCourses = await context.StudentCourses
            .Where(sc => sc.State == EntityState.Active)
            .Include(sc => sc.Role)
            .Include(sc => sc.Course)
            .ToListAsync();
        
        return new ReadAllResponse<StudentCourseEntity>(Responses.Success, studentCourses);
    }

    public async Task<ReadAllResponse<StudentCourseEntity>> GetByStudentIdAsync(Guid studentId)
    {
        var studentCourses = await context.StudentCourses
            .Where(sc => sc.RoleId == studentId && sc.State == EntityState.Active)
            .Include(sc => sc.Role)
            .Include(sc => sc.Course)
            .ToListAsync();
        
        return new ReadAllResponse<StudentCourseEntity>(Responses.Success, studentCourses);
    }

    public async Task<ReadAllResponse<StudentCourseEntity>> GetByCourseIdAsync(Guid courseId)
    {
        var studentCourses = await context.StudentCourses
            .Where(sc => sc.CourseId == courseId && sc.State == EntityState.Active)
            .Include(sc => sc.Role)
            .Include(sc => sc.Course)
            .ToListAsync();
        
        return new ReadAllResponse<StudentCourseEntity>(Responses.Success, studentCourses);
    }

    public async Task<ReadOneResponse<StudentCourseEntity>> GetByIdAsync(Guid id)
    {
        var studentCourse = await context.StudentCourses
            .Include(sc => sc.Role)
            .Include(sc => sc.Course)
            .FirstOrDefaultAsync(sc => sc.Id == id && sc.State == EntityState.Active);
        
        if (studentCourse != null)
            return new ReadOneResponse<StudentCourseEntity>(Responses.Success, studentCourse);
        
        return new ReadOneResponse<StudentCourseEntity>(Responses.NotRows);
    }

    public async Task<ResponseBase> DeleteAsync(Guid id)
    {
        var studentCourse = await context.StudentCourses.FindAsync(id);
        
        if (studentCourse == null)
            return new ResponseBase(Responses.NotRows);
        
        studentCourse.State = EntityState.Deleted;
        await context.SaveChangesAsync();
        
        return new ResponseBase(Responses.Success);
    }

    public async Task<ResponseBase> ExistAsync(Guid id)
    {
        bool exists = await context.StudentCourses
            .AnyAsync(sc => sc.Id == id && sc.State == EntityState.Active);
        
        return new ResponseBase(exists ? Responses.Success : Responses.NotRows);
    }
}
