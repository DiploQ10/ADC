using ADC.Domain.Responses;
using ADC.Persistence.Data;
using ADC.Persistence.Models;
using EntityState = ADC.Persistence.Models.Enums.EntityState;
using Microsoft.EntityFrameworkCore;

namespace ADC.Persistence.Repositories.EF;

internal class CourseFeedbackRepository(DataContext context) : ICourseFeedbackRepository
{
    public async Task<CreateResponse> CreateAsync(CourseFeedbackEntity model)
    {
        model.Id = Guid.CreateVersion7();
        model.CreatedAt = DateTime.UtcNow;
        model.State = EntityState.Active;
        
        context.CourseFeedbacks.Add(model);
        await context.SaveChangesAsync();
        
        return new CreateResponse(Responses.Success, model.Id, "Retroalimentación creada exitosamente");
    }

    public async Task<ReadAllResponse<CourseFeedbackEntity>> GetAllAsync()
    {
        var feedbacks = await context.CourseFeedbacks
            .Where(f => f.State == EntityState.Active)
            .Include(f => f.Student)
            .ToListAsync();
        
        return new ReadAllResponse<CourseFeedbackEntity>(Responses.Success, feedbacks);
    }

    public async Task<ReadAllResponse<CourseFeedbackEntity>> GetByCourseIdAsync(Guid courseId)
    {
        var feedbacks = await context.CourseFeedbacks
            .Where(f => f.State == EntityState.Active)
            .Include(f => f.Student)
                .ThenInclude(s => s.Course)
            .Where(f => f.Student.CourseId == courseId)
            .ToListAsync();
        
        return new ReadAllResponse<CourseFeedbackEntity>(Responses.Success, feedbacks);
    }

    public async Task<ReadAllResponse<CourseFeedbackEntity>> GetByStudentIdAsync(Guid studentId)
    {
        var feedbacks = await context.CourseFeedbacks
            .Where(f => f.StudentId == studentId && f.State == EntityState.Active)
            .Include(f => f.Student)
            .ToListAsync();
        
        return new ReadAllResponse<CourseFeedbackEntity>(Responses.Success, feedbacks);
    }

    public async Task<ReadOneResponse<CourseFeedbackEntity>> GetByIdAsync(Guid id)
    {
        var feedback = await context.CourseFeedbacks
            .Include(f => f.Student)
            .FirstOrDefaultAsync(f => f.Id == id && f.State == EntityState.Active);
        
        if (feedback != null)
            return new ReadOneResponse<CourseFeedbackEntity>(Responses.Success, feedback);
        
        return new ReadOneResponse<CourseFeedbackEntity>(Responses.NotRows);
    }

    public async Task<ResponseBase> UpdateAsync(CourseFeedbackEntity model)
    {
        var existing = await context.CourseFeedbacks.FindAsync(model.Id);
        
        if (existing == null || existing.State == EntityState.Deleted)
            return new ResponseBase(Responses.NotRows);
        
        existing.Rating = model.Rating;
        existing.Comment = model.Comment;
        
        await context.SaveChangesAsync();
        
        return new ResponseBase(Responses.Success);
    }

    public async Task<ResponseBase> DeleteAsync(Guid id)
    {
        var feedback = await context.CourseFeedbacks.FindAsync(id);
        
        if (feedback == null)
            return new ResponseBase(Responses.NotRows);
        
        feedback.State = EntityState.Deleted;
        await context.SaveChangesAsync();
        
        return new ResponseBase(Responses.Success);
    }

    public async Task<ResponseBase> ExistAsync(Guid id)
    {
        bool exists = await context.CourseFeedbacks
            .AnyAsync(f => f.Id == id && f.State == EntityState.Active);
        
        return new ResponseBase(exists ? Responses.Success : Responses.NotRows);
    }
}
