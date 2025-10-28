namespace ADC.Persistence.Models;

public class UserEntity : BaseEntity
{
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; } 
    public required string IdentityDocument { get; set; }
    public required string Password { get; set; }
    public List<UserRoleEntity> Roles { get; set; } = [];
    public DateOnly? Birthday { get; set; }
}