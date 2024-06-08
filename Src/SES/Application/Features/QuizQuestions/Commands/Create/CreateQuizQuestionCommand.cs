using Application.Features.QuizQuestions.Constants;
using Application.Features.QuizQuestions.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using Domain.Enums;
using static Application.Features.QuizQuestions.Constants.QuizQuestionsOperationClaims;

namespace Application.Features.QuizQuestions.Commands.Create;

public class CreateQuizQuestionCommand : IRequest<CreatedQuizQuestionResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public required int BenefitId { get; set; }
    public required int ExamId { get; set; }
    public required int Score { get; set; }
    public required string Question { get; set; }
    public required string QuestionBody { get; set; }
    public required string QuestionImage { get; set; }
    public required QuestionType QuestionType { get; set; }

    public string[] Roles => [Admin, Write, QuizQuestionsOperationClaims.Create];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetQuizQuestions"];

    public class CreateQuizQuestionCommandHandler : IRequestHandler<CreateQuizQuestionCommand, CreatedQuizQuestionResponse>
    {
        private readonly IMapper _mapper;
        private readonly IQuizQuestionRepository _quizQuestionRepository;
        private readonly QuizQuestionBusinessRules _quizQuestionBusinessRules;

        public CreateQuizQuestionCommandHandler(IMapper mapper, IQuizQuestionRepository quizQuestionRepository,
                                         QuizQuestionBusinessRules quizQuestionBusinessRules)
        {
            _mapper = mapper;
            _quizQuestionRepository = quizQuestionRepository;
            _quizQuestionBusinessRules = quizQuestionBusinessRules;
        }

        public async Task<CreatedQuizQuestionResponse> Handle(CreateQuizQuestionCommand request, CancellationToken cancellationToken)
        {
            QuizQuestion quizQuestion = _mapper.Map<QuizQuestion>(request);

            await _quizQuestionRepository.AddAsync(quizQuestion);

            CreatedQuizQuestionResponse response = _mapper.Map<CreatedQuizQuestionResponse>(quizQuestion);
            return response;
        }
    }
}