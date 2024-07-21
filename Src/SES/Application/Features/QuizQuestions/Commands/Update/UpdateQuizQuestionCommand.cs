using Application.Features.QuizQuestions.Constants;
using Application.Features.QuizQuestions.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.QuizQuestions.Constants.QuizQuestionsOperationClaims;

namespace Application.Features.QuizQuestions.Commands.Update;

public class UpdateQuizQuestionCommand : IRequest<UpdatedQuizQuestionResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }
    public required int BenefitId { get; set; }
    public required int ExamId { get; set; }
    public required int Score { get; set; }
    public required string Question { get; set; }
    public required string QuestionBody { get; set; }
    public required string QuestionImage { get; set; }
    public required QuestionType QuestionType { get; set; }

    public string[] Roles => [Admin, Write, QuizQuestionsOperationClaims.Update];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetQuizQuestions"];

    public class UpdateQuizQuestionCommandHandler : IRequestHandler<UpdateQuizQuestionCommand, UpdatedQuizQuestionResponse>
    {
        private readonly IMapper _mapper;
        private readonly IQuizQuestionRepository _quizQuestionRepository;
        private readonly QuizQuestionBusinessRules _quizQuestionBusinessRules;

        public UpdateQuizQuestionCommandHandler(IMapper mapper, IQuizQuestionRepository quizQuestionRepository,
                                         QuizQuestionBusinessRules quizQuestionBusinessRules)
        {
            _mapper = mapper;
            _quizQuestionRepository = quizQuestionRepository;
            _quizQuestionBusinessRules = quizQuestionBusinessRules;
        }

        public async Task<UpdatedQuizQuestionResponse> Handle(UpdateQuizQuestionCommand request, CancellationToken cancellationToken)
        {
            QuizQuestion? quizQuestion = await _quizQuestionRepository.GetAsync(predicate: qq => qq.Id == request.Id, cancellationToken: cancellationToken);
            await _quizQuestionBusinessRules.QuizQuestionShouldExistWhenSelected(quizQuestion);
            quizQuestion = _mapper.Map(request, quizQuestion);

            await _quizQuestionRepository.UpdateAsync(quizQuestion!);

            UpdatedQuizQuestionResponse response = _mapper.Map<UpdatedQuizQuestionResponse>(quizQuestion);
            return response;
        }
    }
}