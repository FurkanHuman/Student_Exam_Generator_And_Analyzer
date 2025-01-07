using Application.Features.QuestionOptions.Constants;
using Application.Features.QuestionOptions.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.QuestionOptions.Constants.QuestionOptionsOperationClaims;

namespace Application.Features.QuestionOptions.Commands.Update;

public class UpdateQuestionOptionCommand : IRequest<UpdatedQuestionOptionResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public required int QuizQuestionId { get; set; }
    public string? OptionText { get; set; }
    public required bool IsCorrect { get; set; }

    

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetQuestionOptions"];

    public class UpdateQuestionOptionCommandHandler : IRequestHandler<UpdateQuestionOptionCommand, UpdatedQuestionOptionResponse>
    {
        private readonly IMapper _mapper;
        private readonly IQuestionOptionRepository _questionOptionRepository;
        private readonly QuestionOptionBusinessRules _questionOptionBusinessRules;

        public UpdateQuestionOptionCommandHandler(IMapper mapper, IQuestionOptionRepository questionOptionRepository,
                                         QuestionOptionBusinessRules questionOptionBusinessRules)
        {
            _mapper = mapper;
            _questionOptionRepository = questionOptionRepository;
            _questionOptionBusinessRules = questionOptionBusinessRules;
        }

        public async Task<UpdatedQuestionOptionResponse> Handle(UpdateQuestionOptionCommand request, CancellationToken cancellationToken)
        {
            QuestionOption? questionOption = await _questionOptionRepository.GetAsync(predicate: qo => qo.Id == request.Id, cancellationToken: cancellationToken);
            await _questionOptionBusinessRules.QuestionOptionShouldExistWhenSelected(questionOption);
            questionOption = _mapper.Map(request, questionOption);

            await _questionOptionRepository.UpdateAsync(questionOption!);

            UpdatedQuestionOptionResponse response = _mapper.Map<UpdatedQuestionOptionResponse>(questionOption);
            return response;
        }
    }
}