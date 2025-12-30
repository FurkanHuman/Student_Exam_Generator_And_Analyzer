using NArchitecture.Core.Application.Responses;

namespace Application.Features.ExamConfigurations.Commands.Delete;

public class DeletedExamConfigurationResponse : IResponse
{
    public int Id { get; set; }
}