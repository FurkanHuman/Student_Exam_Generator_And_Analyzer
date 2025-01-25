using NArchitecture.Core.Application.Responses;

namespace Application.Features.Benefits.Commands.Create;

public class CreatedBenefitResponse : IResponse
{
    public int Id { get; set; }
    public string BenefitCode{ get; set; }
    public string Description { get; set; }
}