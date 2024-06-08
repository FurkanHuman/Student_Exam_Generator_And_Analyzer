using Application.Features.QuestionOptions.Constants;
using Application.Features.QuestionOptions.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.QuestionOptions.Constants.QuestionOptionsOperationClaims;

namespace Application.Features.QuestionOptions.Commands.Create;

public class CreateQuestionOptionCommand : IRequest<CreatedQuestionOptionResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public required int QuizQuestionId { get; set; }
    public string? OptionText { get; set; }
    public required bool IsCorrect { get; set; }

    public string[] Roles => [Admin, Write, QuestionOptionsOperationClaims.Create];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetQuestionOptions"];

    public class CreateQuestionOptionCommandHandler : IRequestHandler<CreateQuestionOptionCommand, CreatedQuestionOptionResponse>
    {
        private readonly IMapper _mapper;
        private readonly IQuestionOptionRepository _questionOptionRepository;
        private readonly QuestionOptionBusinessRules _questionOptionBusinessRules;

        public CreateQuestionOptionCommandHandler(IMapper mapper, IQuestionOptionRepository questionOptionRepository,
                                         QuestionOptionBusinessRules questionOptionBusinessRules)
        {
            _mapper = mapper;
            _questionOptionRepository = questionOptionRepository;
            _questionOptionBusinessRules = questionOptionBusinessRules;
        }

        public async Task<CreatedQuestionOptionResponse> Handle(CreateQuestionOptionCommand request, CancellationToken cancellationToken)
        {
            QuestionOption questionOption = _mapper.Map<QuestionOption>(request);

            await _questionOptionRepository.AddAsync(questionOption);

            CreatedQuestionOptionResponse response = _mapper.Map<CreatedQuestionOptionResponse>(questionOption);
            return response;
        }
    }
}