using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.QuizQuestions.Constants.QuizQuestionsOperationClaims;

namespace Application.Features.QuizQuestions.Queries.GetList;

public class GetListQuizQuestionQuery : IRequest<GetListResponse<GetListQuizQuestionListItemDto>>, ISecuredRequest, ICachableRequest
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [Admin, Read];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListQuizQuestions({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetQuizQuestions";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListQuizQuestionQueryHandler : IRequestHandler<GetListQuizQuestionQuery, GetListResponse<GetListQuizQuestionListItemDto>>
    {
        private readonly IQuizQuestionRepository _quizQuestionRepository;
        private readonly IMapper _mapper;

        public GetListQuizQuestionQueryHandler(IQuizQuestionRepository quizQuestionRepository, IMapper mapper)
        {
            _quizQuestionRepository = quizQuestionRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListQuizQuestionListItemDto>> Handle(GetListQuizQuestionQuery request, CancellationToken cancellationToken)
        {
            IPaginate<QuizQuestion> quizQuestions = await _quizQuestionRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListQuizQuestionListItemDto> response = _mapper.Map<GetListResponse<GetListQuizQuestionListItemDto>>(quizQuestions);
            return response;
        }
    }
}