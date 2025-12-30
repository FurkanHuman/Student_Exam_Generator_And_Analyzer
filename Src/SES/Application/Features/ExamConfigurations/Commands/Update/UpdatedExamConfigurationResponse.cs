using NArchitecture.Core.Application.Responses;

namespace Application.Features.ExamConfigurations.Commands.Update;

public class UpdatedExamConfigurationResponse : IResponse
{
    public int Id { get; set; }
    public string ConfigurationHash { get; set; }
}