using NArchitecture.Core.Application.Dtos;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.Lessons.Queries.GetListLessonQueryBySemesterId;

public class GetListLessonQueryBySemesterIdDto : IDto
{
    public int Id { get; set; }
    public string LessonName { get; set; }
    public string Description { get; set; }
    public int StudentClassAge { get; set; }
}
