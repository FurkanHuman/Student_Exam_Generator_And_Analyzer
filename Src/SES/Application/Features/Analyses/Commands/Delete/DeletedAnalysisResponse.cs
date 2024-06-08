using NArchitecture.Core.Application.Responses;

namespace Application.Features.Analyses.Commands.Delete;

public class DeletedAnalysisResponse : IResponse
{
    public int Id { get; set; }
}