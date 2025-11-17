using Application.Services.Analyses;
using Application.Services.PdfFactory.CreateExamPdf.DTOs;
using Application.Services.PdfFactory.CreateExamPdf.Helpers;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Application.Services.CalculatorFactory;

public class AnalysisDetailTableCalculator
{
    private readonly IAnalysisService _analysisService;

    public AnalysisDetailTableCalculator(IAnalysisService analysisService)
    {
        _analysisService = analysisService;
    }

    public async Task<IList<AnalysisDetailTableDto>> CalculateAsync(int analysisId, CancellationToken cancellationToken = default)
    {
        Analysis? analysis = await GetAnalysisByIdAsync(analysisId, cancellationToken);

        if (analysis == null)
            throw new InvalidOperationException($"Analysis with Id {analysisId} not found.");

        List<AnalysisDetailTableDto> result = [.. (analysis.Exams ?? Enumerable.Empty<Exam>())
            .Select(exam =>
            {
                string examCode = exam.ExamCode;

                uint seed = QuizQuestionHelpers.DecodeBase32String(examCode);

                ExamInfo? examInfo = DeserializeExamInfo(exam);

                List<StudentTableDto> studentDtos = GetStudentTableDtos(exam, analysis, (int)seed, examInfo);

                int questionCount = GetQuestionCount(studentDtos);

                return new AnalysisDetailTableDto
                {
                    ExamCode = examCode,
                    QuestionCount = questionCount,
                    Students = studentDtos
                };
            })];

        return result;
    }

    private static ExamInfo? DeserializeExamInfo(Exam exam)
    {
        try
        {
            return !string.IsNullOrEmpty(exam.ExamConfigurationStr)
                ? JsonSerializer.Deserialize<ExamInfo>(exam.ExamConfigurationStr)
                : new ExamInfo
                {
                    QQOrder = new Dictionary<int, int>(),
                    IsRandomizeQuestions = false,
                    IsRandomizeOptions = false
                };
        }
        catch
        {
            return new ExamInfo
            {
                QQOrder = new Dictionary<int, int>(),
                IsRandomizeQuestions = false,
                IsRandomizeOptions = false
            };
        }
    }

    private static int GetQuestionCount(List<StudentTableDto> studentDtos)
    {
        return studentDtos
            .SelectMany(s => s.StudentAnswerScores)
            .Select(sa => sa.QuestionId)
            .Distinct()
            .Count();
    }

    private static List<StudentTableDto> GetStudentTableDtos(Exam exam, Analysis analysis, int seed, ExamInfo? examInfo)
    {
        return [.. (analysis.StudentExamAnswers ?? Enumerable.Empty<StudentExamAnswer>())
                    .Where(sea => sea.ExamId == exam.Id)
                    .Select(sea =>
                    {
                        List<StudentAnswer> studentAnswers = [.. (sea.StudentAnswers ?? Enumerable.Empty<StudentAnswer>()).Where(sa => sa.QuizQuestion != null)];

                        List<QuizQuestion> quizQuestions = [.. studentAnswers
                            .Select(sa => sa.QuizQuestion)
                            .Where(qq => qq != null)
                            .Distinct()];


                        List<int> orderedQuestionIds = GetOrderedQuestionIds(quizQuestions, seed, examInfo);
                        List<StudentAnswerScore?> studentAnswerScores = [.. orderedQuestionIds
                            .Select((questionId, index) =>
                            {
                                StudentAnswer? studentAnswer = studentAnswers.FirstOrDefault(sa => sa.QuizQuestionId == questionId);

                                return studentAnswer != null ? new StudentAnswerScore
                                {
                                    QuestionId = questionId,
                                    StudentAnswerId = studentAnswer.Id,
                                    GivenScore = studentAnswer.GivenScore ?? 0,
                                    StudentId = studentAnswer.StudentExamAnswer.StudentId
                                } : null;
                            })
                            .Where(s => s != null)];

                        return new StudentTableDto
                        {
                            Id = sea.Student.Id,
                            Name = sea.Student.Name,
                            Surname = sea.Student.SurName,
                            SchoolNumber = sea.Student.SchoolNumber,
                            StudentAnswerScores = studentAnswerScores!
                        };
                    })
                    .OrderBy(s => s.SchoolNumber)];
    }

    private static List<int> GetOrderedQuestionIds(List<QuizQuestion> quizQuestions, int seed, ExamInfo? examInfo)
    {
        List<QuizQuestion> orderedQuestions = ApplySameOrderingLogic(quizQuestions, seed, examInfo);
        return [.. orderedQuestions.Select(qq => qq.Id)];
    }

    private static List<QuizQuestion> ApplySameOrderingLogic(List<QuizQuestion> quizQuestions, int seed, ExamInfo? examInfo)
    {
        if (examInfo == null)
        {
            return [.. quizQuestions.OrderBy(q => q.Id)
                                    .ThenBy(q => new Random(seed * quizQuestions.Count).Next(int.MinValue, int.MaxValue))];
        }

        IList<QuizQuestion> questions = [.. quizQuestions];

        if (examInfo.IsRandomizeQuestions)
        {
            questions = [.. questions.OrderBy(q => q.Id)];
            Random rnd = new Random(seed * questions.Count);
            questions = [.. questions.OrderBy(q => rnd.Next(int.MinValue, int.MaxValue))];
        }

        if (examInfo.QQOrder != null && examInfo.QQOrder.Count > 0)
        {
            Random random = new Random(seed);

            List<KeyValuePair<int, int>> orderedPairs = [.. examInfo.QQOrder.OrderBy(kvp => kvp.Key)];
            HashSet<int> orderedIds = [.. orderedPairs.Select(x => x.Value)];

            List<QuizQuestion?> ordered = [.. orderedPairs
                .Select(p => questions.FirstOrDefault(q => q.Id == p.Value))
                .Where(q => q != null)];

            List<QuizQuestion> unordered = [.. questions
                .Where(q => !orderedIds.Contains(q.Id))
                .OrderBy(q => random.Next())];

            foreach (QuizQuestion q in unordered)
            {
                int insertIndex = random.Next(0, ordered.Count + 1);
                ordered.Insert(insertIndex, q);
            }

            questions = [.. ordered.Cast<QuizQuestion>()];
        }

        return [.. questions];
    }

    private async Task<Analysis?> GetAnalysisByIdAsync(int analysisId, CancellationToken cancellationToken)
    {
        return await _analysisService.GetAsync(
            predicate: a => a.Id == analysisId,
            include: a => a
                .Include(a => a.Exams)
                    .ThenInclude(e => e.QuizQuestions)
                .Include(a => a.StudentExamAnswers)
                    .ThenInclude(sea => sea.Student)
                .Include(a => a.StudentExamAnswers)
                    .ThenInclude(sea => sea.StudentAnswers)
                        .ThenInclude(sa => sa.QuizQuestion),

            cancellationToken: cancellationToken
        );
    }
}

public record StudentTableDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Surname { get; init; } = string.Empty;
    public string SchoolNumber { get; init; } = string.Empty;
    public IList<StudentAnswerScore> StudentAnswerScores { get; init; } = [];
}

public record AnalysisDetailTableDto
{
    public string ExamCode { get; init; } = string.Empty;
    public int QuestionCount { get; init; }
    public IList<StudentTableDto> Students { get; init; } = [];
}

public record StudentAnswerScore
{
    public int QuestionId { get; init; }
    public Guid StudentAnswerId { get; init; }
    public int GivenScore { get; init; }
    public int StudentId { get; init; }
}
