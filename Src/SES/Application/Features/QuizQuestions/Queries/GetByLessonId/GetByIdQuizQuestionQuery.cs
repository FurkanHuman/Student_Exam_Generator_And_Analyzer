using Application.Features.QuizQuestions.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.QuizQuestions.Queries.GetByLessonId;

public class GetByLessonIdQuizQuestionQuery : IRequest<GetListResponse<GetByLessonIdQuizQuestionListItemDto>>
{
    public int LessonId { get; set; }

    public class GetByLessonIdQuizQuestionQueryHandler : IRequestHandler<GetByLessonIdQuizQuestionQuery, GetListResponse<GetByLessonIdQuizQuestionListItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IQuizQuestionRepository _quizQuestionRepository;
        private readonly QuizQuestionBusinessRules _quizQuestionBusinessRules;

        public GetByLessonIdQuizQuestionQueryHandler(IMapper mapper, IQuizQuestionRepository quizQuestionRepository, QuizQuestionBusinessRules quizQuestionBusinessRules)
        {
            _mapper = mapper;
            _quizQuestionRepository = quizQuestionRepository;
            _quizQuestionBusinessRules = quizQuestionBusinessRules;
        }

        public async Task<GetListResponse<GetByLessonIdQuizQuestionListItemDto>> Handle(GetByLessonIdQuizQuestionQuery request, CancellationToken cancellationToken)
        {
            IPaginate<QuizQuestion> quizQuestion = await _quizQuestionRepository.GetListAsync(predicate: qq => qq.Lessons.Any(l => l.Id == request.LessonId),
                                                                                              include: qq => qq.Include(qq => qq.Lessons)
                                                                                                               .Include(qq => qq.Options)
                                                                                                               .Include(qq => qq.QuestionScore)
                                                                                                               .Include(qq => qq.Benefits),
                                                                                              size: int.MaxValue,
                                                                                              cancellationToken: cancellationToken);

            await _quizQuestionBusinessRules.QuizQuestionShouldExistWhenSelected(quizQuestion);

            GetListResponse<GetByLessonIdQuizQuestionListItemDto> response = _mapper.Map<GetListResponse<GetByLessonIdQuizQuestionListItemDto>>(quizQuestion);
            return response;
        }
    }
}