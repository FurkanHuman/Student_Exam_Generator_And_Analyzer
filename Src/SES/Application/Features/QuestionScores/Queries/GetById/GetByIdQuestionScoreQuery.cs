using Application.Features.QuestionScores.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.QuestionScores.Queries.GetById;

public class GetByIdQuestionScoreQuery : IRequest<GetByIdQuestionScoreResponse>
{
    public int Id { get; set; }



    public class GetByIdQuestionScoreQueryHandler : IRequestHandler<GetByIdQuestionScoreQuery, GetByIdQuestionScoreResponse>
    {
        private readonly IMapper _mapper;
        private readonly IQuestionScoreRepository _questionScoreRepository;
        private readonly QuestionScoreBusinessRules _questionScoreBusinessRules;

        public GetByIdQuestionScoreQueryHandler(IMapper mapper, IQuestionScoreRepository questionScoreRepository, QuestionScoreBusinessRules questionScoreBusinessRules)
        {
            _mapper = mapper;
            _questionScoreRepository = questionScoreRepository;
            _questionScoreBusinessRules = questionScoreBusinessRules;
        }

        public async Task<GetByIdQuestionScoreResponse> Handle(GetByIdQuestionScoreQuery request, CancellationToken cancellationToken)
        {
            QuestionScore? questionScore = await _questionScoreRepository.GetAsync(predicate: qs => qs.Id == request.Id, cancellationToken: cancellationToken);
            await _questionScoreBusinessRules.QuestionScoreShouldExistWhenSelected(questionScore);

            GetByIdQuestionScoreResponse response = _mapper.Map<GetByIdQuestionScoreResponse>(questionScore);
            return response;
        }
    }
}