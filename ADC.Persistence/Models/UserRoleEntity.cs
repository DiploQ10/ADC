namespace ADC.Persistence.Models;

public class UserRoleEntity : BaseEntity
{
    public UserRole Role  {get;set;}
    public required UserEntity User { get; set; }
    public Guid UserId { get; set; }
}