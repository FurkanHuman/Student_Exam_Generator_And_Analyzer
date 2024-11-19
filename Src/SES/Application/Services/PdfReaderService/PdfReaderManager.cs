using Application.Services.PdfReaderService.Dtos;
using iText.IO.Source;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Application.Services.PdfReaderService;
internal class PdfReaderManager : IPdfReaderService
{
    private readonly string _classRegex = @"(?:(Anasınıfı)|(\d+)\. Sınıf)\s*/\s*([A-Z])\s*Şubesi";
    private readonly string _studentRegex = @"^(?<ogrenciNo>\d+)\s+(?<adSoyad>[A-Za-zğüşöçİĞÜŞÖÇ]+\s[A-Za-zğüşöçİĞÜŞÖÇ]+(?:\s[A-Za-zğüşöçİĞÜŞÖÇ]+)*)\s+(?<cinsiyet>Kız|Erkek)$";

    public async Task<ICollection<ClassWithStudentsDto>> GetAllClassesAndStudents(byte[] pdfBytes)
    {
        ConcurrentBag<ClassWithStudentsDto> result = [];

        IRandomAccessSource byteSource = new RandomAccessSourceFactory().CreateSource(pdfBytes);
        PdfReader pdfReader = new(byteSource, new ReaderProperties());
        PdfDocument pdfDocument = new(pdfReader);

        try
        {
            ICollection<Task> tasks = [];
            for (int pageNumber = 1; pageNumber <= pdfDocument.GetNumberOfPages(); pageNumber++)
                tasks.Add(ProcessPageAsync(pdfDocument, pageNumber, result));


            await Task.WhenAll(tasks);
        }

        finally
        {
            pdfDocument.Close();
            pdfReader.Close();
            byteSource.Close();
        }

        return [.. result];
    }


    private async Task ProcessPageAsync(PdfDocument pdfDocument, int pageNumber, ConcurrentBag<ClassWithStudentsDto> result)
    {
        string pageContent = ExtractTextFromPage(pdfDocument, pageNumber);

        ClassWithStudentsDto? classWithStudents = ExtractClassAndStudentsFromPage(pageContent);

        if (classWithStudents != null)
        {
            result.Add(classWithStudents);
        }

        await Task.Yield();
    }

    private string ExtractTextFromPage(PdfDocument pdfDoc, int pageNumber)
    {
        SimpleTextExtractionStrategy strategy = new();
        return PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(pageNumber), strategy);
    }

    private ClassWithStudentsDto? ExtractClassAndStudentsFromPage(string content)
    {
        Regex classRegex = new Regex(_classRegex);
        Match classMatch = classRegex.Match(content);

        if (!classMatch.Success)
            return null;

        int age = classMatch.Groups[1].Success ? 0 : int.Parse(classMatch.Groups[2].Value);
        char section = classMatch.Groups[3].Value[0];

        return new ClassWithStudentsDto
        {
            Age = age,
            Section = section,
            Students = ExtractStudentInfo(content)
        };
    }

    private ICollection<string[]> ExtractStudentInfo(string content)
    {
        ICollection<string[]> ogrenciler = [];

        Regex studentRegex = new Regex(_studentRegex, RegexOptions.Multiline);

        string[] lines = content.Split('\n');

        foreach (var line in lines)
        {
            string trimmedLine = line.Trim();

            if (string.IsNullOrWhiteSpace(trimmedLine) ||
                trimmedLine.StartsWith("T.C.") ||
                trimmedLine.StartsWith("Kız Öğrenci Sayısı"))
                continue;

            Match match = studentRegex.Match(trimmedLine);
            if (!match.Success) continue;

            string ogrenciNoStr = match.Groups["ogrenciNo"].Value;
            if (string.IsNullOrEmpty(ogrenciNoStr) || !int.TryParse(ogrenciNoStr, out int ogrenciNo))
                continue;

            string cinsiyet = match.Groups["cinsiyet"].Value;
            string adSoyad = match.Groups["adSoyad"].Value.Trim();

            // Ad ve soyad ayırma
            string[] adSoyadParts = adSoyad.Split(' ');
            string soyad = adSoyadParts[^1];
            string ad = string.Join(" ", adSoyadParts[..^1]);

            ogrenciler.Add([ogrenciNoStr, ad, soyad, cinsiyet]);
        }

        return ogrenciler;
    }
}
