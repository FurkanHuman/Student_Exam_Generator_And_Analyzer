using NArchitecture.Core.Application.Responses;

namespace Application.Features.Lessons.Commands.Create;

public class CreatedLessonResponse : IResponse
{
    public int Id { get; set; }
    public string LessonName { get; set; }
    public string Description { get; set; }
    public int SemesterId { get; set; }
}