using Application.Services.PdfReaderService.Dtos;
using Domain.Entities;

namespace Application.Services.PdfReaderService;
public interface IPdfReaderService
{
    Task<ICollection<ClassWithStudentsDto>> GetAllClassesAndStudents(byte[] pdfBytes);
    Task<ICollection<RawBenefitReferenceDto>> GetAllReferenceBenefitsWithAltersDtos(byte[] pdfBytes);
    ReferenceBenefitDto ProcessReferenceBenefit(ICollection<string> lines);
}
