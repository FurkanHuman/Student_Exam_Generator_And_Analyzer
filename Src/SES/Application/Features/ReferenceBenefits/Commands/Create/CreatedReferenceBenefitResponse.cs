using NArchitecture.Core.Application.Responses;

namespace Application.Features.ReferenceBenefits.Commands.Create;

public class CreatedReferenceBenefitResponse : IResponse
{
    public int Id { get; set; }
    public string ReferenceBenefitName { get; set; }
    public int SemesterId { get; set; }
    public int SchoolId { get; set; }
    public int ExamId { get; set; }
    public int LearningAreaId { get; set; }

}