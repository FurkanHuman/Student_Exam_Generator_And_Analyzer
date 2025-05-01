namespace Application.Services.PdfFactory.PdfReaderService.Dtos;

public record ReferenceBenefitDto
{
    public required string RBName { get; set; }
    public int LessonId { get; set; }
    public int SchoolId { get; set; }
    public int SemesterId { get; set; }
    public required ICollection<LearningAreaDto> LearningAreas { get; set; }
}
