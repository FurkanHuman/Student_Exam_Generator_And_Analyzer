using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Lessons.Queries.GetList;

public class GetListLessonListItemDto : IDto
{
    public int Id { get; set; }
    public string LessonName { get; set; }
    public string Description { get; set; }
    public int SemesterId { get; set; }
}