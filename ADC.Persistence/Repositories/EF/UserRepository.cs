using ADC.Domain.Responses;
using ADC.Persistence.Data;
using ADC.Persistence.Models;
using EntityState = ADC.Persistence.Models.Enums.EntityState;
using Microsoft.EntityFrameworkCore;

namespace ADC.Persistence.Repositories.EF;

public class UserRepository(DataContext context) : IUserRepository
{
    public async Task<CreateResponse> CreateAsync(UserEntity model)
    {
        model.Id = Guid.CreateVersion7();
        model.CreatedAt = DateTime.UtcNow;
        model.State = EntityState.Active;
        
        context.Users.Add(model);
        await context.SaveChangesAsync();

        return new CreateResponse(Responses.Success, model.Id, "Usuario creado exitosamente");
    }

    public async Task<ResponseBase> Delete(Guid id)
    {
        var user = await context.Users.FindAsync(id);
        
        if (user == null)
            return new ResponseBase(Responses.NotRows);
        
        user.State = EntityState.Deleted;
        await context.SaveChangesAsync();
        
        return new ResponseBase(Responses.Success);
    }

    public async Task<ResponseBase> ExistAsync(Guid id)
    {
        bool exists = await context.Users
            .AnyAsync(u => u.Id == id && u.State == EntityState.Active);
        
        return new ResponseBase(exists ? Responses.Success : Responses.NotRows);
    }

    public async Task<ReadAllResponse<UserEntity>> GetAllAsync()
    {
        var users = await context.Users
            .Where(u => u.State == EntityState.Active)
            .Include(u => u.Roles)
            .ToListAsync();

        return new ReadAllResponse<UserEntity>(Responses.Success, users);
    }

    public async Task<ReadOneResponse<UserEntity>> GetByIdAsync(Guid id)
    {
        var user = await context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == id && u.State == EntityState.Active);
        
        if (user != null)
            return new ReadOneResponse<UserEntity>(Responses.Success, user);

        return new ReadOneResponse<UserEntity>(Responses.NotRows);
    }

    public async Task<ResponseBase> Update(UserEntity model)
    {
        var existing = await context.Users.FindAsync(model.Id);
        
        if (existing == null || existing.State == EntityState.Deleted)
            return new ResponseBase(Responses.NotRows);
        
        existing.Name = model.Name;
        existing.LastName = model.LastName;
        existing.Email = model.Email;
        existing.IdentityDocument = model.IdentityDocument;
        existing.Birthday = model.Birthday;
        
        // No actualizar Password aquí - usar endpoint separado para cambio de contraseña
        
        await context.SaveChangesAsync();
        
        return new ResponseBase(Responses.Success);
    }
}
