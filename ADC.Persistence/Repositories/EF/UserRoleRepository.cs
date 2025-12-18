using ADC.Domain.Responses;
using ADC.Persistence.Data;
using ADC.Persistence.Models;
using EntityState = ADC.Persistence.Models.Enums.EntityState;
using Microsoft.EntityFrameworkCore;

namespace ADC.Persistence.Repositories.EF;

internal class UserRoleRepository(DataContext context) : IUserRoleRepository
{
    public async Task<CreateResponse> CreateAsync(UserRoleEntity model)
    {
        model.Id = Guid.CreateVersion7();
        model.CreatedAt = DateTime.UtcNow;
        model.State = EntityState.Active;
        
        context.UserRoles.Add(model);
        await context.SaveChangesAsync();
        
        return new CreateResponse(Responses.Success, model.Id, "Rol de usuario creado exitosamente");
    }

    public async Task<ReadAllResponse<UserRoleEntity>> GetAllAsync()
    {
        var userRoles = await context.UserRoles
            .Where(ur => ur.State == EntityState.Active)
            .Include(ur => ur.User)
            .ToListAsync();
        
        return new ReadAllResponse<UserRoleEntity>(Responses.Success, userRoles);
    }

    public async Task<ReadAllResponse<UserRoleEntity>> GetByUserIdAsync(Guid userId)
    {
        var userRoles = await context.UserRoles
            .Where(ur => ur.UserId == userId && ur.State == EntityState.Active)
            .Include(ur => ur.User)
            .ToListAsync();
        
        return new ReadAllResponse<UserRoleEntity>(Responses.Success, userRoles);
    }

    public async Task<ReadOneResponse<UserRoleEntity>> GetByIdAsync(Guid id)
    {
        var userRole = await context.UserRoles
            .Include(ur => ur.User)
            .FirstOrDefaultAsync(ur => ur.Id == id && ur.State == EntityState.Active);
        
        if (userRole != null)
            return new ReadOneResponse<UserRoleEntity>(Responses.Success, userRole);
        
        return new ReadOneResponse<UserRoleEntity>(Responses.NotRows);
    }

    public async Task<ResponseBase> UpdateAsync(UserRoleEntity model)
    {
        var existing = await context.UserRoles.FindAsync(model.Id);
        
        if (existing == null || existing.State == EntityState.Deleted)
            return new ResponseBase(Responses.NotRows);
        
        existing.Role = model.Role;
        
        await context.SaveChangesAsync();
        
        return new ResponseBase(Responses.Success);
    }

    public async Task<ResponseBase> DeleteAsync(Guid id)
    {
        var userRole = await context.UserRoles.FindAsync(id);
        
        if (userRole == null)
            return new ResponseBase(Responses.NotRows);
        
        userRole.State = EntityState.Deleted;
        await context.SaveChangesAsync();
        
        return new ResponseBase(Responses.Success);
    }

    public async Task<ResponseBase> ExistAsync(Guid id)
    {
        bool exists = await context.UserRoles
            .AnyAsync(ur => ur.Id == id && ur.State == EntityState.Active);
        
        return new ResponseBase(exists ? Responses.Success : Responses.NotRows);
    }
}
