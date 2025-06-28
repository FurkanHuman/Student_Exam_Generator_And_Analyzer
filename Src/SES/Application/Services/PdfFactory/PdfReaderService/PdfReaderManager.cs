using Application.Services.PdfFactory.PdfReaderService.Dtos;
using iText.IO.Source;
using iText.Kernel.Pdf;
using System.Collections.Concurrent;

namespace Application.Services.PdfFactory.PdfReaderService;
internal class PdfReaderManager(PdfReaderStudentManager pdfReadeRStudent) : IPdfReaderService
{
    private readonly PdfReaderStudentManager _pdfReadeRStudent = pdfReadeRStudent;

    public async Task<ICollection<ClassWithStudentsDto>> GetAllClassesAndStudents(byte[] pdfBytes)
    {
        ConcurrentBag<ClassWithStudentsDto> classWithStudentsBag = [];

        IRandomAccessSource byteSource = new RandomAccessSourceFactory().CreateSource(pdfBytes);
        using PdfReader pdfReader = new(byteSource, new ReaderProperties());
        using PdfDocument pdfDocument = new(pdfReader);

        try
        {
            ICollection<Task> tasks = [];
            for (int pageNumber = 1; pageNumber <= pdfDocument.GetNumberOfPages(); pageNumber++)
                tasks.Add(_pdfReadeRStudent.ProcessPageAsync(pdfDocument, pageNumber, classWithStudentsBag));

            await Task.WhenAll(tasks);
        }

        finally
        {
            pdfDocument.Close();
            pdfReader.Close();
            byteSource.Close();
        }

        return [.. classWithStudentsBag];
    }

    public async Task<ICollection<RawBenefitReferenceDto>> GetAllReferenceBenefitsWithAltersDtos(byte[] pdfBytes)
    {
        ConcurrentBag<RawBenefitReferenceDto> rawBenefitReferencesBag = [];

        IRandomAccessSource byteSource = new RandomAccessSourceFactory().CreateSource(pdfBytes);
        using PdfReader pdfReader = new(byteSource, new ReaderProperties());
        using PdfDocument pdfDocument = new(pdfReader);

        try
        {
            ICollection<Task> tasks = [];
            for (int pageNumber = 1; pageNumber <= pdfDocument.GetNumberOfPages(); pageNumber++)
                tasks.Add(PdfReaderReferenceBenefitManager.ProcessPageAsync(pdfDocument, pageNumber, rawBenefitReferencesBag));
            await Task.WhenAll(tasks);
        }

        finally
        {
            pdfDocument.Close();
            pdfReader.Close();
            byteSource.Close();
        }

        ICollection<RawBenefitReferenceDto> result = PdfReaderReferenceBenefitManager.DistinctByAllItems(rawBenefitReferencesBag);

        return [.. result];
    }

    public ReferenceBenefitDto ProcessReferenceBenefit(ICollection<string> lines) => PdfReaderReferenceBenefitManager.ProcessReferenceBenefit(lines);
}
