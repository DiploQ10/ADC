namespace ADC.Persistence.Models;

using ADC.Persistence.Models.Enums;

public class UserRoleEntity : BaseEntity
{
    public required UserRole Role { get; set; }
    public required UserEntity User { get; set; }
    public Guid UserId { get; set; }
}