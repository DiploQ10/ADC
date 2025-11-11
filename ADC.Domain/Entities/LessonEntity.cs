namespace ADC.Domain.Entities;

public class LessonEntity : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public LessonType Type { get; set; }
    public int Order { get; set; }
    public string? Url { get; set; }

    public required SectionEntity Section {  get; set; }
    public Guid SectionId { get; set; }
}