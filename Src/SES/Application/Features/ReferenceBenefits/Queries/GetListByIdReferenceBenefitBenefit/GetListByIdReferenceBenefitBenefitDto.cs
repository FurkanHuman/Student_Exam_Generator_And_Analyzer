using NArchitecture.Core.Application.Dtos;

namespace Application.Features.ReferenceBenefits.Queries.GetListByIdReferenceBenefitBenefit;

public class GetListByIdReferenceBenefitBenefitDto : IDto
{
    public int Id { get; set; }
    public string BenefitCode { get; set; }
    public string Description { get; set; }

}