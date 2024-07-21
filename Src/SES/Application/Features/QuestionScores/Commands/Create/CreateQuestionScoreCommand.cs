using Application.Features.QuestionScores.Constants;
using Application.Features.QuestionScores.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.QuestionScores.Constants.QuestionScoresOperationClaims;

namespace Application.Features.QuestionScores.Commands.Create;

public class CreateQuestionScoreCommand : IRequest<CreatedQuestionScoreResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public required int Score { get; set; }
    public required int MaxScore { get; set; }

    public string[] Roles => [Admin, Write, QuestionScoresOperationClaims.Create];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetQuestionScores"];

    public class CreateQuestionScoreCommandHandler : IRequestHandler<CreateQuestionScoreCommand, CreatedQuestionScoreResponse>
    {
        private readonly IMapper _mapper;
        private readonly IQuestionScoreRepository _questionScoreRepository;
        private readonly QuestionScoreBusinessRules _questionScoreBusinessRules;

        public CreateQuestionScoreCommandHandler(IMapper mapper, IQuestionScoreRepository questionScoreRepository,
                                         QuestionScoreBusinessRules questionScoreBusinessRules)
        {
            _mapper = mapper;
            _questionScoreRepository = questionScoreRepository;
            _questionScoreBusinessRules = questionScoreBusinessRules;
        }

        public async Task<CreatedQuestionScoreResponse> Handle(CreateQuestionScoreCommand request, CancellationToken cancellationToken)
        {
            QuestionScore questionScore = _mapper.Map<QuestionScore>(request);

            await _questionScoreRepository.AddAsync(questionScore);

            CreatedQuestionScoreResponse response = _mapper.Map<CreatedQuestionScoreResponse>(questionScore);
            return response;
        }
    }
}