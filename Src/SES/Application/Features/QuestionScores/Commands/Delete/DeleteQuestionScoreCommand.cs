using Application.Features.QuestionScores.Constants;
using Application.Features.QuestionScores.Constants;
using Application.Features.QuestionScores.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.QuestionScores.Constants.QuestionScoresOperationClaims;

namespace Application.Features.QuestionScores.Commands.Delete;

public class DeleteQuestionScoreCommand : IRequest<DeletedQuestionScoreResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }

    public string[] Roles => [Admin, Write, QuestionScoresOperationClaims.Delete];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetQuestionScores"];

    public class DeleteQuestionScoreCommandHandler : IRequestHandler<DeleteQuestionScoreCommand, DeletedQuestionScoreResponse>
    {
        private readonly IMapper _mapper;
        private readonly IQuestionScoreRepository _questionScoreRepository;
        private readonly QuestionScoreBusinessRules _questionScoreBusinessRules;

        public DeleteQuestionScoreCommandHandler(IMapper mapper, IQuestionScoreRepository questionScoreRepository,
                                         QuestionScoreBusinessRules questionScoreBusinessRules)
        {
            _mapper = mapper;
            _questionScoreRepository = questionScoreRepository;
            _questionScoreBusinessRules = questionScoreBusinessRules;
        }

        public async Task<DeletedQuestionScoreResponse> Handle(DeleteQuestionScoreCommand request, CancellationToken cancellationToken)
        {
            QuestionScore? questionScore = await _questionScoreRepository.GetAsync(predicate: qs => qs.Id == request.Id, cancellationToken: cancellationToken);
            await _questionScoreBusinessRules.QuestionScoreShouldExistWhenSelected(questionScore);

            await _questionScoreRepository.DeleteAsync(questionScore!);

            DeletedQuestionScoreResponse response = _mapper.Map<DeletedQuestionScoreResponse>(questionScore);
            return response;
        }
    }
}