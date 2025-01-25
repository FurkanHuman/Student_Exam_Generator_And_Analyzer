using NArchitecture.Core.Application.Responses;

namespace Application.Features.Benefits.Commands.MultiCreate;

public class MultiCreatedBenefitResponse : IResponse
{
    public int Id { get; set; }
    public string BenefitCode { get; set; }
    public string Description { get; set; }
}