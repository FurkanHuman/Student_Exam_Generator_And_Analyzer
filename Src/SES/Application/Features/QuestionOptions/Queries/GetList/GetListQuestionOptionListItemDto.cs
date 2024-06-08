using NArchitecture.Core.Application.Dtos;

namespace Application.Features.QuestionOptions.Queries.GetList;

public class GetListQuestionOptionListItemDto : IDto
{
    public Guid Id { get; set; }
    public int QuizQuestionId { get; set; }
    public string? OptionText { get; set; }
    public bool IsCorrect { get; set; }
}