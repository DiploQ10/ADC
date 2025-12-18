using ADC.Domain.Responses;
using ADC.Persistence.Models;

namespace ADC.Persistence.Repositories;

public interface IUserRoleRepository
{
    Task<CreateResponse> CreateAsync(UserRoleEntity model);
    Task<ReadAllResponse<UserRoleEntity>> GetAllAsync();
    Task<ReadAllResponse<UserRoleEntity>> GetByUserIdAsync(Guid userId);
    Task<ReadOneResponse<UserRoleEntity>> GetByIdAsync(Guid id);
    Task<ResponseBase> UpdateAsync(UserRoleEntity model);
    Task<ResponseBase> DeleteAsync(Guid id);
    Task<ResponseBase> ExistAsync(Guid id);
}
