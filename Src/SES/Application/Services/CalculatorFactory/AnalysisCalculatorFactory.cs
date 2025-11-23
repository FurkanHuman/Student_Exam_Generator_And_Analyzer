using Application.Services.Analyses;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.CalculatorFactory;

public abstract class AnalysisCalculatorFactory<TResult>(IAnalysisService analysisService) : IAnalysisCalculatorFactory<TResult>
{
    protected readonly IAnalysisService _analysisService = analysisService;
    
    protected int PassingScore { get; private set; }

    public virtual async Task<TResult> CalculateAsync(int analysisId, CancellationToken cancellationToken = default)
    {
        await LoadPassingScoreAsync(analysisId, cancellationToken);

        return default!;
    }

    protected async Task LoadPassingScoreAsync(int analysisId, CancellationToken cancellationToken = default)
    {
        Analysis? analysis = await _analysisService.GetAsync(
                                                              predicate: a => a.Id == analysisId,
                                                              include: a => a.Include(a => a.Lesson),
                                                              cancellationToken: cancellationToken
                                                             );

        if (analysis?.Lesson != null)
            PassingScore = analysis.Lesson.PassingScore;

        else
            PassingScore = 50; // Default fallback

    }
}