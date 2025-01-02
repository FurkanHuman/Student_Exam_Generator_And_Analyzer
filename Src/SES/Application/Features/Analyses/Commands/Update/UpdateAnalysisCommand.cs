using Application.Features.Analyses.Constants;
using Application.Features.Analyses.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Analyses.Constants.AnalysesOperationClaims;

namespace Application.Features.Analyses.Commands.Update;

public class UpdateAnalysisCommand : IRequest<UpdatedAnalysisResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }
    public required int ClassAge { get; set; }
    public required char AltClass { get; set; }
    public required string ExamSemesterYear { get; set; }
    public required string LessonName { get; set; }
    public required string LessonSession { get; set; }
    public required string ExamCode { get; set; }
    public required string FooterNote { get; set; }
    public required int SemesterId { get; set; }
    public required int BenefitId { get; set; }
    public required int QuestionId { get; set; }
    public required int TeacherId { get; set; }
    public required int PrincipalId { get; set; }
    public required int StudentAnswerId { get; set; }
    public required int SchoolId { get; set; }
    public required Semester Semester { get; set; }
    public required Teacher Teacher { get; set; }
    public required Principal Principal { get; set; }
    public required School School { get; set; }

    

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetAnalyses"];

    public class UpdateAnalysisCommandHandler : IRequestHandler<UpdateAnalysisCommand, UpdatedAnalysisResponse>
    {
        private readonly IMapper _mapper;
        private readonly IAnalysisRepository _analysisRepository;
        private readonly AnalysisBusinessRules _analysisBusinessRules;

        public UpdateAnalysisCommandHandler(IMapper mapper, IAnalysisRepository analysisRepository,
                                         AnalysisBusinessRules analysisBusinessRules)
        {
            _mapper = mapper;
            _analysisRepository = analysisRepository;
            _analysisBusinessRules = analysisBusinessRules;
        }

        public async Task<UpdatedAnalysisResponse> Handle(UpdateAnalysisCommand request, CancellationToken cancellationToken)
        {
            Analysis? analysis = await _analysisRepository.GetAsync(predicate: a => a.Id == request.Id, cancellationToken: cancellationToken);
            await _analysisBusinessRules.AnalysisShouldExistWhenSelected(analysis);
            analysis = _mapper.Map(request, analysis);

            await _analysisRepository.UpdateAsync(analysis!);

            UpdatedAnalysisResponse response = _mapper.Map<UpdatedAnalysisResponse>(analysis);
            return response;
        }
    }
}