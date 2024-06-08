using NArchitecture.Core.Application.Dtos;

namespace Application.Features.ReferenceBenefits.Queries.GetList;

public class GetListReferenceBenefitListItemDto : IDto
{
    public int Id { get; set; }
    public string ReferenceBenefitName { get; set; }
    public int SemesterId { get; set; }
    public int SchoolId { get; set; }
    public int ExamId { get; set; }
    public int LearningAreaId { get; set; }
}