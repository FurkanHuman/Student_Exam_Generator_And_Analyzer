using Application.Services.PdfFactory.PdfReaderService.Dtos;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Application.Services.PdfFactory.PdfReaderService;

internal class PdfReaderStudentManager
{
    private const string _classRegex = @"(?:(Anasınıfı)|(\d+)\. Sınıf)\s*/\s*([A-Z])\s*Şubesi";
    
    #pragma warning disable S1144 // Unused private types or members should be removed
    private const string _studentRegex = @"^(?<ogrenciNo>\d+)\s+(?<adSoyad>[A-Za-zğüşöçİĞÜŞÖÇ]+\s[A-Za-zğüşöçİĞÜŞÖÇ]+(?:\s[A-Za-zğüşöçİĞÜŞÖÇ]+)*)\s+(?<cinsiyet>Kız|Erkek)$";
    #pragma warning restore S1144 // Unused private types or members should be removed
    
    private const string _studentNewRegex = @"^(?<ogrenciNo>\d+)\s+(?<ad>[A-ZĞÜŞÖÇİ]+(?:\s[A-ZĞÜŞÖÇİ]+)*)\s+(?<cinsiyet>Kız|Erkek)\s+(?<soyad>[A-ZĞÜŞÖÇİ]+)$";

    private ClassWithStudentsDto? ExtractClassAndStudentsFromPage(string content)
    {
        var classRegex = new Regex(_classRegex);
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
        ICollection<string[]> students = [];

        var studentRegex = new Regex(_studentNewRegex, RegexOptions.Multiline);

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

            string studentNumberStr = match.Groups["ogrenciNo"].Value;
            if (string.IsNullOrEmpty(studentNumberStr) || !int.TryParse(studentNumberStr, out int ogrenciNo))
                continue;

            string gender = match.Groups["cinsiyet"].Value;
            string name = match.Groups["ad"].Value;
            string surname = match.Groups["soyad"].Value;

            students.Add([ogrenciNo.ToString(), name, surname, gender]);
        }

        return students;
    }

    private string ExtractTextFromPage(PdfDocument pdfDoc, int pageNumber)
    {
        SimpleTextExtractionStrategy strategy = new();
        return PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(pageNumber), strategy);
    }

    internal async Task ProcessPageAsync(PdfDocument pdfDocument, int pageNumber, ConcurrentBag<ClassWithStudentsDto> result)
    {
        string pageContent = ExtractTextFromPage(pdfDocument, pageNumber);

        ClassWithStudentsDto? classWithStudents = ExtractClassAndStudentsFromPage(pageContent);

        if (classWithStudents != null)
            result.Add(classWithStudents);

        await Task.Yield();
    }
}