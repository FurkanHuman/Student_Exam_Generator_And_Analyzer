using Application.Features.QuestionOptions.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using static Application.Features.QuestionOptions.Constants.QuestionOptionsOperationClaims;

namespace Application.Features.QuestionOptions.Queries.GetById;

public class GetByIdQuestionOptionQuery : IRequest<GetByIdQuestionOptionResponse>
{
    public Guid Id { get; set; }

    

    public class GetByIdQuestionOptionQueryHandler : IRequestHandler<GetByIdQuestionOptionQuery, GetByIdQuestionOptionResponse>
    {
        private readonly IMapper _mapper;
        private readonly IQuestionOptionRepository _questionOptionRepository;
        private readonly QuestionOptionBusinessRules _questionOptionBusinessRules;

        public GetByIdQuestionOptionQueryHandler(IMapper mapper, IQuestionOptionRepository questionOptionRepository, QuestionOptionBusinessRules questionOptionBusinessRules)
        {
            _mapper = mapper;
            _questionOptionRepository = questionOptionRepository;
            _questionOptionBusinessRules = questionOptionBusinessRules;
        }

        public async Task<GetByIdQuestionOptionResponse> Handle(GetByIdQuestionOptionQuery request, CancellationToken cancellationToken)
        {
            QuestionOption? questionOption = await _questionOptionRepository.GetAsync(predicate: qo => qo.Id == request.Id, cancellationToken: cancellationToken);
            await _questionOptionBusinessRules.QuestionOptionShouldExistWhenSelected(questionOption);

            GetByIdQuestionOptionResponse response = _mapper.Map<GetByIdQuestionOptionResponse>(questionOption);
            return response;
        }
    }
}