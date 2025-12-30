using NArchitecture.Core.Application.Responses;

namespace Application.Features.ExamConfigurations.Commands.Create;

public class CreatedExamConfigurationResponse : IResponse
{
    public int Id { get; set; }
    public string ConfigurationHash { get; set; }
}