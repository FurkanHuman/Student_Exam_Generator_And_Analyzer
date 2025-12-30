using NArchitecture.Core.Application.Responses;

namespace Application.Features.ExamConfigurations.Queries.GetById;

public class GetByIdExamConfigurationResponse : IResponse
{
    public int Id { get; set; }
    public string ConfigurationHash { get; set; }
}