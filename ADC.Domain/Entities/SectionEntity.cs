namespace ADC.Domain.Entities;

public class SectionEntity : BaseEntity
{
    public required string Name { get; set; }
    public int Order { get; set; }
    public List<LessonEntity> Lessons { get; set; } = [];
    public required CourseEntity Course { get; set; }
    public Guid CourseId { get; set; }
}