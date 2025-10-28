namespace ADC.Persistence.Models;

public class CourseEntity : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public DateTime? LastUpdated { get; set; }
    public List<StudentCourseEntity> Students { get; set; } = [];
    public List<CourseFeedbackEntity> FeedbackEntities { get; set; } = [];

    public required UserRoleEntity Instructor { get; set; }
    public Guid InstructorId { get; set; }
}