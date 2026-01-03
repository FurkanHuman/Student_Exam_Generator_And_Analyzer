using Application.Features.QuizQuestions.Rules;
using Application.Services.Benefits;
using Application.Services.Lessons;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;

namespace Application.Features.QuizQuestions.Commands.Create;

public class CreateQuizQuestionCommand : IRequest<CreatedQuizQuestionResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public bool IsAIGenerated { get; set; } = false;
    public int LessonId { get; set; }
    public ICollection<int> BenefitIds { get; set; } = [];
    public required int Score { get; set; }
    public required int MaxScore { get; set; }
    public required int RefBenefitId { get; set; }
    public required string Prompt { get; set; }
    public string? Stem { get; set; }
    public string? QuestionImage { get; set; }
    public required QuestionType QuestionType { get; set; }
    public ICollection<QuestionOptionAppDto?> QQOptions { get; set; }
    public int? PreviousQuestionId { get; set; }

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetQuizQuestions"];

    public class CreateQuizQuestionCommandHandler : IRequestHandler<CreateQuizQuestionCommand, CreatedQuizQuestionResponse>
    {
        private readonly IMapper _mapper;
        private readonly IQuizQuestionRepository _quizQuestionRepository;
        private readonly ILessonService _lessonService;
        private readonly IBenefitService _benefitService;
        private readonly QuizQuestionBusinessRules _quizQuestionBusinessRules;

        public CreateQuizQuestionCommandHandler(IMapper mapper, IQuizQuestionRepository quizQuestionRepository, ILessonService lessonService, IBenefitService benefitService, QuizQuestionBusinessRules quizQuestionBusinessRules)
        {
            _mapper = mapper;
            _quizQuestionRepository = quizQuestionRepository;
            _lessonService = lessonService;
            _benefitService = benefitService;
            _quizQuestionBusinessRules = quizQuestionBusinessRules;
        }

        public async Task<CreatedQuizQuestionResponse> Handle(CreateQuizQuestionCommand request, CancellationToken cancellationToken)
        {
            ICollection<Benefit> benefits = [];
            ICollection<Lesson> lessons = [];
            ICollection<QuestionOption> questionOptions = [];

            Lesson? lesson = await _lessonService.GetAsync(l => l.Id == request.LessonId, cancellationToken: cancellationToken);
            lessons.Add(lesson);

            foreach (int id in request.BenefitIds)
                benefits.Add(await _benefitService.GetAsync(predicate: b => b.Id == id, cancellationToken: cancellationToken));

            foreach (QuestionOptionAppDto? option in request.QQOptions)
                questionOptions.Add(new() { IsCorrect = option.IsCorrect, OptionText = option.OptionText, CreatedDate = DateTime.UtcNow });

            QuestionScore questionScore = _mapper.Map<QuestionScore>(request);
            questionScore.CreatedDate = DateTime.UtcNow;

            QuizQuestion quizQuestion = _mapper.Map<QuizQuestion>(request);

            quizQuestion.Options = [.. questionOptions];
            quizQuestion.QuestionScore = questionScore;
            quizQuestion.Lessons = [.. lessons];
            quizQuestion.Benefits = [.. benefits];

            if (request.PreviousQuestionId.HasValue)
            {
                QuizQuestion? preQQ = await _quizQuestionRepository.GetAsync(qq => qq.Id == quizQuestion.PreviousQuestionId, cancellationToken: cancellationToken);
                quizQuestion.QuestionImageURL = preQQ?.QuestionImageURL;
            }



            await _quizQuestionRepository.AddAsync(quizQuestion, cancellationToken);

            CreatedQuizQuestionResponse response = _mapper.Map<CreatedQuizQuestionResponse>(quizQuestion);
            return response;
        }
    }
}