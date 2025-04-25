namespace Application.Services.PdfFactory.PdfReaderService.Dtos;

public record LearningAreaDto
{
    public required string LACode { get; set; }
    public required string Description { get; set; }
    public required ICollection<SubLearningDto> SubLearningAreas { get; set; }
}