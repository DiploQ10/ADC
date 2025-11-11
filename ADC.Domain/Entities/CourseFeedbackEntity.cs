namespace ADC.Domain.Entities;

public class CourseFeedbackEntity : BaseEntity
{
    public float Rating { get; set; }
    public string? Comment { get; set; }
    public required StudentCourseEntity Student { get; set; }
    public Guid StudentId { get; set; }
}