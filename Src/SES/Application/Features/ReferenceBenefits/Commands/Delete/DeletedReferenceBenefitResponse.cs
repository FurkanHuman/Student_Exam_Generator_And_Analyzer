using NArchitecture.Core.Application.Responses;

namespace Application.Features.ReferenceBenefits.Commands.Delete;

public class DeletedReferenceBenefitResponse : IResponse
{
    public int Id { get; set; }
}