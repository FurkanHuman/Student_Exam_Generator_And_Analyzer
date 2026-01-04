using Application.Features.QuizQuestions.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.QuizQuestions.Queries.GetById;

public class GetByIdQuizQuestionQuery : IRequest<GetByIdQuizQuestionResponse>
{
    public int Id { get; set; }

    public class GetByIdQuizQuestionQueryHandler : IRequestHandler<GetByIdQuizQuestionQuery, GetByIdQuizQuestionResponse>
    {
        private readonly IMapper _mapper;
        private readonly IQuizQuestionRepository _quizQuestionRepository;
        private readonly QuizQuestionBusinessRules _quizQuestionBusinessRules;

        public GetByIdQuizQuestionQueryHandler(IMapper mapper, IQuizQuestionRepository quizQuestionRepository, QuizQuestionBusinessRules quizQuestionBusinessRules)
        {
            _mapper = mapper;
            _quizQuestionRepository = quizQuestionRepository;
            _quizQuestionBusinessRules = quizQuestionBusinessRules;
        }

        public async Task<GetByIdQuizQuestionResponse> Handle(GetByIdQuizQuestionQuery request, CancellationToken cancellationToken)
        {
            QuizQuestion? quizQuestion = await _quizQuestionRepository.GetAsync(predicate: qq => qq.Id == request.Id,
                                                                                include:   qq => qq.Include(qq => qq.Options)
                                                                                                   .Include(qq => qq.QuestionScore),
                                                                                cancellationToken: cancellationToken);
            await _quizQuestionBusinessRules.QuizQuestionShouldExistWhenSelected(quizQuestion);

            GetByIdQuizQuestionResponse response = _mapper.Map<GetByIdQuizQuestionResponse>(quizQuestion);
            return response;
        }
    }
}