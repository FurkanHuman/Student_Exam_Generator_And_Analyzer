using NArchitecture.Core.Application.Dtos;

namespace Application.Features.ExamConfigurations.Queries.GetList;

public class GetListExamConfigurationListItemDto : IDto
{
    public int Id { get; set; }
    public string ConfigurationHash { get; set; }
}