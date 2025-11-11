namespace ADC.Domain.Entities;

public class StudentCourseEntity : BaseEntity
{
    public required UserRoleEntity Role { get; set; }
    public required CourseEntity Course { get; set; }

    public Guid RoleId { get; set; }
    public Guid CourseId { get; set; }
}