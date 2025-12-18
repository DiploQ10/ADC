using ADC.Domain.Responses;
using ADC.Persistence.Data;
using ADC.Persistence.Models;
using EntityState = ADC.Persistence.Models.Enums.EntityState;
using Microsoft.EntityFrameworkCore;

namespace ADC.Persistence.Repositories.EF;

internal class LessonRepository(DataContext context) : ILessonRepository
{
    public async Task<CreateResponse> CreateAsync(LessonEntity model)
    {
        model.Id = Guid.CreateVersion7();
        model.CreatedAt = DateTime.UtcNow;
        model.State = EntityState.Active;
        
        context.Lessons.Add(model);
        await context.SaveChangesAsync();
        
        return new CreateResponse(Responses.Success, model.Id, "Lección creada exitosamente");
    }

    public async Task<ReadAllResponse<LessonEntity>> GetAllAsync()
    {
        var lessons = await context.Lessons
            .Where(l => l.State == EntityState.Active)
            .Include(l => l.Section)
            .ToListAsync();
        
        return new ReadAllResponse<LessonEntity>(Responses.Success, lessons);
    }

    public async Task<ReadAllResponse<LessonEntity>> GetBySectionIdAsync(Guid sectionId)
    {
        var lessons = await context.Lessons
            .Where(l => l.SectionId == sectionId && l.State == EntityState.Active)
            .Include(l => l.Section)
            .ToListAsync();
        
        return new ReadAllResponse<LessonEntity>(Responses.Success, lessons);
    }

    public async Task<ReadOneResponse<LessonEntity>> GetByIdAsync(Guid id)
    {
        var lesson = await context.Lessons
            .Include(l => l.Section)
            .FirstOrDefaultAsync(l => l.Id == id && l.State == EntityState.Active);
        
        if (lesson != null)
            return new ReadOneResponse<LessonEntity>(Responses.Success, lesson);
        
        return new ReadOneResponse<LessonEntity>(Responses.NotRows);
    }

    public async Task<ResponseBase> UpdateAsync(LessonEntity model)
    {
        var existing = await context.Lessons.FindAsync(model.Id);
        
        if (existing == null || existing.State == EntityState.Deleted)
            return new ResponseBase(Responses.NotRows);
        
        existing.Name = model.Name;
        existing.Description = model.Description;
        existing.Type = model.Type;
        existing.Order = model.Order;
        existing.Url = model.Url;
        
        await context.SaveChangesAsync();
        
        return new ResponseBase(Responses.Success);
    }

    public async Task<ResponseBase> DeleteAsync(Guid id)
    {
        var lesson = await context.Lessons.FindAsync(id);
        
        if (lesson == null)
            return new ResponseBase(Responses.NotRows);
        
        lesson.State = EntityState.Deleted;
        await context.SaveChangesAsync();
        
        return new ResponseBase(Responses.Success);
    }

    public async Task<ResponseBase> ExistAsync(Guid id)
    {
        bool exists = await context.Lessons
            .AnyAsync(l => l.Id == id && l.State == EntityState.Active);
        
        return new ResponseBase(exists ? Responses.Success : Responses.NotRows);
    }
}
