using Application.Features.QuestionScores.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;

namespace Application.Features.QuestionScores.Commands.Update;

public class UpdateQuestionScoreCommand : IRequest<UpdatedQuestionScoreResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }
    public required int Score { get; set; }
    public required int MaxScore { get; set; }



    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetQuestionScores"];

    public class UpdateQuestionScoreCommandHandler : IRequestHandler<UpdateQuestionScoreCommand, UpdatedQuestionScoreResponse>
    {
        private readonly IMapper _mapper;
        private readonly IQuestionScoreRepository _questionScoreRepository;
        private readonly QuestionScoreBusinessRules _questionScoreBusinessRules;

        public UpdateQuestionScoreCommandHandler(IMapper mapper, IQuestionScoreRepository questionScoreRepository,
                                         QuestionScoreBusinessRules questionScoreBusinessRules)
        {
            _mapper = mapper;
            _questionScoreRepository = questionScoreRepository;
            _questionScoreBusinessRules = questionScoreBusinessRules;
        }

        public async Task<UpdatedQuestionScoreResponse> Handle(UpdateQuestionScoreCommand request, CancellationToken cancellationToken)
        {
            QuestionScore? questionScore = await _questionScoreRepository.GetAsync(predicate: qs => qs.Id == request.Id, cancellationToken: cancellationToken);
            await _questionScoreBusinessRules.QuestionScoreShouldExistWhenSelected(questionScore);
            questionScore = _mapper.Map(request, questionScore);

            await _questionScoreRepository.UpdateAsync(questionScore!);

            UpdatedQuestionScoreResponse response = _mapper.Map<UpdatedQuestionScoreResponse>(questionScore);
            return response;
        }
    }
}