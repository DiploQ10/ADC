using ADC.Domain.Entities;
using ADC.Domain.Responses;

namespace ADC.Domain.Repositories;

public interface IUserRepository
{
    Task<CreateResponse> CreateAsync(UserEntity model);
    Task<ReadAllResponse<UserEntity>> GetAllAsync();
    Task<ReadOneResponse<UserEntity>> GetByIdAsync(Guid id);
    Task<ResponseBase> ExistAsync(Guid id);
    Task<ReadOneResponse<UserEntity>> GetByEmail(string email);
    Task<ResponseBase> Update(UserEntity model);
    Task<ResponseBase> Delete(Guid id);
}