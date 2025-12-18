using ADC.Domain.Responses;
using ADC.Persistence.Data;
using ADC.Persistence.Models;
using EntityState = ADC.Persistence.Models.Enums.EntityState;
using Microsoft.EntityFrameworkCore;

namespace ADC.Persistence.Repositories.EF;

internal class SectionRepository(DataContext context) : ISectionRepository
{
    public async Task<CreateResponse> CreateAsync(SectionEntity model)
    {
        model.Id = Guid.CreateVersion7();
        model.CreatedAt = DateTime.UtcNow;
        model.State = EntityState.Active;
        
        context.Sections.Add(model);
        await context.SaveChangesAsync();
        
        return new CreateResponse(Responses.Success, model.Id, "Sección creada exitosamente");
    }

    public async Task<ReadAllResponse<SectionEntity>> GetAllAsync()
    {
        var sections = await context.Sections
            .Where(s => s.State == EntityState.Active)
            .Include(s => s.Course)
            .ToListAsync();
        
        return new ReadAllResponse<SectionEntity>(Responses.Success, sections);
    }

    public async Task<ReadAllResponse<SectionEntity>> GetByCourseIdAsync(Guid courseId)
    {
        var sections = await context.Sections
            .Where(s => s.CourseId == courseId && s.State == EntityState.Active)
            .Include(s => s.Course)
            .Include(s => s.Lessons)
            .ToListAsync();
        
        return new ReadAllResponse<SectionEntity>(Responses.Success, sections);
    }

    public async Task<ReadOneResponse<SectionEntity>> GetByIdAsync(Guid id)
    {
        var section = await context.Sections
            .Include(s => s.Course)
            .Include(s => s.Lessons)
            .FirstOrDefaultAsync(s => s.Id == id && s.State == EntityState.Active);
        
        if (section != null)
            return new ReadOneResponse<SectionEntity>(Responses.Success, section);
        
        return new ReadOneResponse<SectionEntity>(Responses.NotRows);
    }

    public async Task<ResponseBase> UpdateAsync(SectionEntity model)
    {
        var existing = await context.Sections.FindAsync(model.Id);
        
        if (existing == null || existing.State == EntityState.Deleted)
            return new ResponseBase(Responses.NotRows);
        
        existing.Name = model.Name;
        existing.Order = model.Order;
        
        await context.SaveChangesAsync();
        
        return new ResponseBase(Responses.Success);
    }

    public async Task<ResponseBase> DeleteAsync(Guid id)
    {
        var section = await context.Sections.FindAsync(id);
        
        if (section == null)
            return new ResponseBase(Responses.NotRows);
        
        section.State = EntityState.Deleted;
        await context.SaveChangesAsync();
        
        return new ResponseBase(Responses.Success);
    }

    public async Task<ResponseBase> ExistAsync(Guid id)
    {
        bool exists = await context.Sections
            .AnyAsync(s => s.Id == id && s.State == EntityState.Active);
        
        return new ResponseBase(exists ? Responses.Success : Responses.NotRows);
    }
}
