using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Exams.Queries.GetListExamByLessonId;

public class GetListExamByLessonIdDto : IDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public DateOnly ExamDate { get; set; }
    public string ExamCode { get; set; }
    public string ExamConfigurationStr { get; set; }
    public IEnumerable<int> QuizQuestionsIds { get; set; }
}
