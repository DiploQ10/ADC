using ADC.Domain.Responses;
using ADC.Persistence.Models;

namespace ADC.Persistence.Repositories;

public interface IUserRepository
{
    Task<CreateResponse> CreateAsync(UserEntity model);
    Task<ReadAllResponse<UserEntity>> GetAllAsync();
    Task<ReadOneResponse<UserEntity>> GetByIdAsync(Guid id);
    Task<ReadOneResponse<UserEntity>> GetByUsernameAsync(string username);
    Task<ReadOneResponse<UserEntity>> GetByEmailAsync(string email);
    Task<ResponseBase> ExistAsync(Guid id);
    Task<ResponseBase> Update(UserEntity model);
    Task<ResponseBase> Delete(Guid id);
}