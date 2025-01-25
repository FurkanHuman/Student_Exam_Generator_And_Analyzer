namespace Application.Services.PdfReaderService.Dtos;

public record BenefitDto
{
    public required string BCode { get; set; }
    public required string Description { get; set; }
}
