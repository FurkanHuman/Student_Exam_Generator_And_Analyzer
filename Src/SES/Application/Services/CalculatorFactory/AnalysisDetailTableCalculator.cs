using Application.Services.Analyses;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Services.CalculatorFactory
{
    public class AnalysisDetailTableCalculator
    {
        private readonly IAnalysisService _analysisService;

        public AnalysisDetailTableCalculator(IAnalysisService analysisService)
        {
            _analysisService = analysisService;
        }

        public async Task<IList<AnalysisDetailTableDto>> CalculateAsync(int analysisId, CancellationToken cancellationToken = default)
        {
            IPaginate<Analysis>? analyses = await GetAnalysesByIdAsync(analysisId, cancellationToken);

            Analysis? analysis = analyses?.Items?.FirstOrDefault();

            if (analysis == null)
                throw new InvalidOperationException($"Analysis with Id {analysisId} not found.");

            List<AnalysisDetailTableDto> result = [.. (analysis.Exams ?? Enumerable.Empty<Exam>())
                .Select(exam =>
                {
                    string examCode = exam.ExamCode; List<StudentTableDto> studentDtos = GetStudentTableDtos(exam, analysis);

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

        private static int GetQuestionCount(List<StudentTableDto> studentDtos)
        {
            return studentDtos
                .SelectMany(s => s.StudentAnswerScores)
                .Select(sa => sa.QuestionId)
                .Distinct()
                .Count();
        }

        private static List<StudentTableDto> GetStudentTableDtos(Exam exam, Analysis analysis)
        {
            return [.. (analysis.StudentExamAnswers ?? Enumerable.Empty<StudentExamAnswer>())
                        .Where(sea => sea.ExamId == exam.Id)
                        .Select(sea => new StudentTableDto
                        {
                            Id = sea.Student.Id,
                            Name = sea.Student.Name,
                            Surname = sea.Student.SurName,
                            SchoolNumber = sea.Student.SchoolNumber,
                            StudentAnswerScores = [.. (sea.StudentAnswers ?? Enumerable.Empty<StudentAnswer>())
                                .Where(sa => sa.QuizQuestion != null)
                                .Select(sa => new StudentAnswerScore
                                {
                                    QuestionId = sa.QuizQuestionId,
                                    StudentAnswerId = sa.Id,
                                    GivenScore = sa.GivenScore ?? 0,
                                    StudentId = sa.StudentExamAnswer.StudentId
                                })
                                .OrderBy(s => s.QuestionId)]
                        })
                        .OrderBy(s => s.SchoolNumber)];
        }

        private async Task<IPaginate<Analysis>?> GetAnalysesByIdAsync(int analysisId, CancellationToken cancellationToken)
        {
            return await _analysisService.GetListAsync(
                predicate: a => a.Id == analysisId,
                include: a => a
                    .Include(a => a.Exams)
                    .Include(a => a.StudentExamAnswers)
                        .ThenInclude(sea => sea.Student)
                    .Include(a => a.StudentExamAnswers)
                        .ThenInclude(sea => sea.StudentAnswers)
                            .ThenInclude(sa => sa.QuizQuestion),
                index: 0,
                size: int.MaxValue,
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
}
