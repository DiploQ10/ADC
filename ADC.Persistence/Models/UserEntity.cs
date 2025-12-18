namespace ADC.Persistence.Models;

public class UserEntity : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; } 
    public required string IdentityDocument { get; set; }
    public required string Password { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginAt { get; set; }
    public List<UserRoleEntity> Roles { get; set; } = [];
    public DateOnly? Birthday { get; set; }
}