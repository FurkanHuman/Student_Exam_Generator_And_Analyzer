namespace Application.Services.PdfFactory.CreateExamPdf.DTOs;

public class ExamInfo
{
    public IDictionary<int, int> QQOrder { get; set; } // key is question Id, value is exam question order
    public DateOnly ExamScheduledDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.Date);
    public string ExamName { get; set; }
    public string Language { get; set; } = "TUR"; // exam language
    public string FooterNote { get; set; } // footer note for the exam
    public string? ExamScoreStr { get; set; } = "Yüz"; // score in string format
    public int ExamScore { get; set; } = 100;
    public int SelectedClass { get; set; } = 0;
    public byte ExamTerm { get; set; } // period of exam
    public byte CurrentExamNumber { get; set; } // in period number of exam 
    public bool IsStandardOptionMode { get; set; } = true;
    public bool IsRandomizeQuestions { get; set; } = false;
    public bool IsRandomizeOptions { get; set; } = false;
    public bool IsOpticCodeForm { get; set; } = false; // back page optic code form
    public bool IsAnonymousExamMode { get; set; } = true;
    public bool IsCommonExam { get; set; } = true;
    public bool IsGhostExam { get; set; } = false; // exam without student info and readers do not know who they are.
    public bool PracticeExam { get; set; } = false; // practice exam 
    public bool ShowWatermark { get; set; } = true;
}
