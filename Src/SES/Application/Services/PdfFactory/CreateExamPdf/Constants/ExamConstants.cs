namespace Application.Services.PdfFactory.CreateExamPdf.Constants;
internal static class ExamConstants
{
    private static readonly IDictionary<string, IDictionary<string, string>> _examConstants = new Dictionary<string, IDictionary<string, string>>
    {
        { "TUR", new Dictionary<string, string>
            {
                { "score", "Puan" },
                { "exams", "Sınavlar" },
                { "period", "Dönem" },
                { "written_exam", "Yazılı Sınav" },
                { "clasess", "Sınıflar" },
                { "lesson", "Dersi" }
            }
        },

        { "ENG", new Dictionary<string, string>
            {
                { "score", "Score" },
                { "exams", "Exams" },
                { "period","Period" },
                { "written_exam", "Written Exam" },
                { "clasess", "Classes" },
                { "lesson", "Lesson" }
            }
        }
    };

    public static string CurrentLanguage { get; set; } = "TUR";

    public static string Get(string key)
    {
        if (_examConstants.TryGetValue(CurrentLanguage, out var langDict) &&
            langDict.TryGetValue(key, out var value))
            return value;
        return key;
    }
}
