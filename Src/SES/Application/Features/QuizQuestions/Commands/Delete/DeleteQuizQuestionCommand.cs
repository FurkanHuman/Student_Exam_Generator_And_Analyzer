using Application.Features.QuizQuestions.Constants;
using Application.Features.QuizQuestions.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.QuizQuestions.Constants.QuizQuestionsOperationClaims;

namespace Application.Features.QuizQuestions.Commands.Delete;

public class DeleteQuizQuestionCommand : IRequest<DeletedQuizQuestionResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }

    public string[] Roles => [Admin, Write, QuizQuestionsOperationClaims.Delete];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetQuizQuestions"];

    public class DeleteQuizQuestionCommandHandler : IRequestHandler<DeleteQuizQuestionCommand, DeletedQuizQuestionResponse>
    {
        private readonly IMapper _mapper;
        private readonly IQuizQuestionRepository _quizQuestionRepository;
        private readonly QuizQuestionBusinessRules _quizQuestionBusinessRules;

        public DeleteQuizQuestionCommandHandler(IMapper mapper, IQuizQuestionRepository quizQuestionRepository,
                                         QuizQuestionBusinessRules quizQuestionBusinessRules)
        {
            _mapper = mapper;
            _quizQuestionRepository = quizQuestionRepository;
            _quizQuestionBusinessRules = quizQuestionBusinessRules;
        }

        public async Task<DeletedQuizQuestionResponse> Handle(DeleteQuizQuestionCommand request, CancellationToken cancellationToken)
        {
            QuizQuestion? quizQuestion = await _quizQuestionRepository.GetAsync(predicate: qq => qq.Id == request.Id, cancellationToken: cancellationToken);
            await _quizQuestionBusinessRules.QuizQuestionShouldExistWhenSelected(quizQuestion);

            await _quizQuestionRepository.DeleteAsync(quizQuestion!);

            DeletedQuizQuestionResponse response = _mapper.Map<DeletedQuizQuestionResponse>(quizQuestion);
            return response;
        }
    }
}