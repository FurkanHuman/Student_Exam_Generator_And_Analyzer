using Application.Services.PdfReaderService.Dtos;

namespace Application.Services.PdfReaderService;
public interface IPdfReaderService
{
    Task<ICollection<ClassWithStudentsDto>> GetAllClassesAndStudents(byte[] pdfBytes);
}