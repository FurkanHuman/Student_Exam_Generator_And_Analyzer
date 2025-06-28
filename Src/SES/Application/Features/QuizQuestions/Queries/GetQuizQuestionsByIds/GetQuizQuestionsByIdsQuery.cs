using Application.Features.QuizQuestions.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.QuizQuestions.Queries.GetQuizQuestionsByIds;

public class GetQuizQuestionsByIdsQuery : IRequest<GetListResponse<GetQuizQuestionsByIdsListItemDto>>
{
    public IEnumerable<int> Ids { get; set; }
    public class GetQuizQuestionsByIdsQueryHandler : IRequestHandler<GetQuizQuestionsByIdsQuery, GetListResponse<GetQuizQuestionsByIdsListItemDto>>
    {
        private readonly IMapper _mapper;
        private readonly IQuizQuestionRepository _quizQuestionRepository;
        private readonly QuizQuestionBusinessRules _quizQuestionBusinessRules;

        public GetQuizQuestionsByIdsQueryHandler(IMapper mapper, IQuizQuestionRepository quizQuestionRepository, QuizQuestionBusinessRules quizQuestionBusinessRules)
        {
            _mapper = mapper;
            _quizQuestionRepository = quizQuestionRepository;
            _quizQuestionBusinessRules = quizQuestionBusinessRules;
        }

        public async Task<GetListResponse<GetQuizQuestionsByIdsListItemDto>> Handle(GetQuizQuestionsByIdsQuery request, CancellationToken cancellationToken)
        {
            IPaginate<QuizQuestion> quizQuestions = await _quizQuestionRepository.GetListAsync(
                predicate: qq => request.Ids.Contains(qq.Id),
                include: qq => qq.Include(qq => qq.Options)
                                 .Include(qq => qq.QuestionScore),
                size: int.MaxValue,
                enableTracking: true,
                cancellationToken: cancellationToken
            );

            await _quizQuestionBusinessRules.QuizQuestionShouldExistWhenSelected(quizQuestions);

            GetListResponse<GetQuizQuestionsByIdsListItemDto> response = _mapper.Map<GetListResponse<GetQuizQuestionsByIdsListItemDto>>(quizQuestions);
            return response;
        }
    }
}
