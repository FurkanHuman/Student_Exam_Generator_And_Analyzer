using Application.Services.PdfFactory.PdfReaderService.Dtos;

namespace Application.Services.PdfFactory.PdfReaderService;

public interface IPdfReaderService
{
    Task<ICollection<ClassWithStudentsDto>> GetAllClassesAndStudents(byte[] pdfBytes);
    Task<ICollection<RawBenefitReferenceDto>> GetAllReferenceBenefitsWithAltersDtos(byte[] pdfBytes);
    ReferenceBenefitDto ProcessReferenceBenefit(ICollection<string> lines);
}
