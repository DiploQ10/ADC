using ADC.Domain.Entities;
using ADC.Domain.Repositories;
using ADC.Domain.Responses;
using ADC.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ADC.Persistence.Repositories;

internal class UserRepository(DataContext context) : IUserRepository
{
    public async Task<CreateResponse> CreateAsync(UserEntity model)
    {
        model.Id = Guid.CreateVersion7();
        context.Add(model);
        await context.SaveChangesAsync();

        return new CreateResponse(Responses.Success, model.Id);
    }

    public async Task<ResponseBase> Delete(Guid id)
    {
        int count = await (from user in context.Users
                           where user.Id == id
                           select user).ExecuteDeleteAsync();

        if (count > 0)
            return new ResponseBase(Responses.Success);
        return new ResponseBase(Responses.Undefined);
    }

    public async Task<ResponseBase> ExistAsync(Guid id)
    {
        bool exists = await context.Users.AnyAsync(t => t.Id == id);
        if (exists)
            return new ResponseBase(Responses.Success);

        return new ResponseBase(Responses.NotRows);
    }

    public async Task<ReadAllResponse<UserEntity>> GetAllAsync()
    {

        var models = await (from user in context.Users
                            select user).ToListAsync();


        return new ReadAllResponse<UserEntity>(Responses.Success, models);
    }

    public async Task<ReadOneResponse<UserEntity>> GetByIdAsync(Guid id)
    {
        var user = context.Users.FirstOrDefault(t => t.Id == id);
        if (user != null)
            return new ReadOneResponse<UserEntity>(Responses.Success, user);

        return new ReadOneResponse<UserEntity>(Responses.NotRows);
    }

    public async Task<ReadOneResponse<UserEntity>> GetByEmail(string email)
    {
        var user = context.Users.FirstOrDefault(t => t.Email == email);
        if (user != null)
            return new ReadOneResponse<UserEntity>(Responses.Success, user);

        return new ReadOneResponse<UserEntity>(Responses.NotRows);
    }

    public async Task<ResponseBase> Update(UserEntity model)
    {
        throw new NotImplementedException();
    }
}
