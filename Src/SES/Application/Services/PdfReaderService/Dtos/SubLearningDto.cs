namespace Application.Services.PdfReaderService.Dtos;

public record SubLearningDto
{
    public required string SLCode { get; set; }
    public required string Description { get; set; }
    public required ICollection<BenefitDto> Benefits { get; set; }
}
