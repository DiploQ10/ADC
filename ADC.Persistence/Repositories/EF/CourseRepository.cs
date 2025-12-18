using ADC.Domain.Responses;
using ADC.Persistence.Data;
using ADC.Persistence.Models;
using EntityState = ADC.Persistence.Models.Enums.EntityState;
using Microsoft.EntityFrameworkCore;

namespace ADC.Persistence.Repositories.EF;

internal class CourseRepository(DataContext context) : ICourseRepository
{
    public async Task<CreateResponse> CreateAsync(CourseEntity model)
    {
        model.Id = Guid.CreateVersion7();
        model.CreatedAt = DateTime.UtcNow;
        model.State = EntityState.Active;
        model.LastUpdated = DateTime.UtcNow;
        
        context.Courses.Add(model);
        await context.SaveChangesAsync();
        
        return new CreateResponse(Responses.Success, model.Id, "Curso creado exitosamente");
    }

    public async Task<ReadAllResponse<CourseEntity>> GetAllAsync()
    {
        var courses = await context.Courses
            .Where(c => c.State == EntityState.Active)
            .Include(c => c.Instructor)
            .ToListAsync();
        
        return new ReadAllResponse<CourseEntity>(Responses.Success, courses);
    }

    public async Task<ReadOneResponse<CourseEntity>> GetByIdAsync(Guid id)
    {
        var course = await context.Courses
            .Include(c => c.Instructor)
            .Include(c => c.Students)
            .Include(c => c.FeedbackEntities)
            .FirstOrDefaultAsync(c => c.Id == id && c.State == EntityState.Active);
        
        if (course != null)
            return new ReadOneResponse<CourseEntity>(Responses.Success, course);
        
        return new ReadOneResponse<CourseEntity>(Responses.NotRows);
    }

    public async Task<ReadAllResponse<CourseEntity>> GetByInstructorIdAsync(Guid instructorId)
    {
        var courses = await context.Courses
            .Where(c => c.InstructorId == instructorId && c.State == EntityState.Active)
            .Include(c => c.Instructor)
            .ToListAsync();
        
        return new ReadAllResponse<CourseEntity>(Responses.Success, courses);
    }

    public async Task<ResponseBase> UpdateAsync(CourseEntity model)
    {
        var existing = await context.Courses.FindAsync(model.Id);
        
        if (existing == null || existing.State == EntityState.Deleted)
            return new ResponseBase(Responses.NotRows);
        
        existing.Name = model.Name;
        existing.Description = model.Description;
        existing.ImageUrl = model.ImageUrl;
        existing.Price = model.Price;
        existing.LastUpdated = DateTime.UtcNow;
        
        await context.SaveChangesAsync();
        
        return new ResponseBase(Responses.Success);
    }

    public async Task<ResponseBase> DeleteAsync(Guid id)
    {
        var course = await context.Courses.FindAsync(id);
        
        if (course == null)
            return new ResponseBase(Responses.NotRows);
        
        course.State = EntityState.Deleted;
        await context.SaveChangesAsync();
        
        return new ResponseBase(Responses.Success);
    }

    public async Task<ResponseBase> ExistAsync(Guid id)
    {
        bool exists = await context.Courses
            .AnyAsync(c => c.Id == id && c.State == EntityState.Active);
        
        return new ResponseBase(exists ? Responses.Success : Responses.NotRows);
    }
}
