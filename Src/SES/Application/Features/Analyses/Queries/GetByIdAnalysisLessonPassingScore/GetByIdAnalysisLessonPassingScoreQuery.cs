using Application.Features.Analyses.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Analyses.Queries.GetByIdAnalysisLessonPassingScore;

public class GetByIdAnalysisLessonPassingScoreQuery : IRequest<int>
{
    public int Id { get; set; }
}

public class GetByIdLessonPassingScoreQueryHandler : IRequestHandler<GetByIdAnalysisLessonPassingScoreQuery, int>
{
    private readonly IAnalysisRepository _analysisRepository;
    private readonly AnalysisBusinessRules _analysisBusinessRules;

    public GetByIdLessonPassingScoreQueryHandler(IAnalysisRepository analysisRepository, AnalysisBusinessRules analysisBusinessRules)
    {
        _analysisRepository = analysisRepository;
        _analysisBusinessRules = analysisBusinessRules;
    }

    public async Task<int> Handle(GetByIdAnalysisLessonPassingScoreQuery request, CancellationToken cancellationToken)
    {
        await _analysisBusinessRules.AnalysisIdShouldExistWhenSelected(request.Id, cancellationToken);

        Analysis? analysis = await _analysisRepository.GetAsync(
                                                                predicate: a => a.Id == request.Id,
                                                                include: a => a.Include(a => a.Lesson),
                                                                cancellationToken: cancellationToken
                                                                );

        await _analysisBusinessRules.AnalysisShouldExistWhenSelected(analysis);

        return analysis!.Lesson.PassingScore;
    }
}
