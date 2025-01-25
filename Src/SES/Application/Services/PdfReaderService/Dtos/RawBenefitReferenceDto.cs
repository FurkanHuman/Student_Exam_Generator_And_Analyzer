namespace Application.Services.PdfReaderService.Dtos;

public class RawBenefitReferenceDto
{
    public ICollection<string>? Benefits { get; set; }
    public ICollection<string>? SubLearningAreas { get; set; }
    public ICollection<string>? LearningAreas { get; set; }
}
