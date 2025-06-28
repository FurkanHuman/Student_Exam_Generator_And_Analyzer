using NArchitecture.Core.Application.Dtos;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.Exams.Queries.GetListExamByLessonId;

public class GetListExamByLessonIdDto : IDto 
{
    public int Id { get; set; }
    public DateOnly ExamDate { get; set; }
    public string ExamCode { get; set; }
    public int StudentId { get; set; }
    public IEnumerable<int> QuizQuestionsIds { get; set; }
}
