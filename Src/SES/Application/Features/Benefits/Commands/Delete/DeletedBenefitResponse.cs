using NArchitecture.Core.Application.Responses;

namespace Application.Features.Benefits.Commands.Delete;

public class DeletedBenefitResponse : IResponse
{
    public int Id { get; set; }
}