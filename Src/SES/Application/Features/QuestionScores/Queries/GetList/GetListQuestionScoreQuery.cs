using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.QuestionScores.Queries.GetList;

public class GetListQuestionScoreQuery : IRequest<GetListResponse<GetListQuestionScoreListItemDto>>, ICachableRequest
{
    public PageRequest PageRequest { get; set; }



    public bool BypassCache { get; }
    public string? CacheKey => $"GetListQuestionScores({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetQuestionScores";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListQuestionScoreQueryHandler : IRequestHandler<GetListQuestionScoreQuery, GetListResponse<GetListQuestionScoreListItemDto>>
    {
        private readonly IQuestionScoreRepository _questionScoreRepository;
        private readonly IMapper _mapper;

        public GetListQuestionScoreQueryHandler(IQuestionScoreRepository questionScoreRepository, IMapper mapper)
        {
            _questionScoreRepository = questionScoreRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListQuestionScoreListItemDto>> Handle(GetListQuestionScoreQuery request, CancellationToken cancellationToken)
        {
            IPaginate<QuestionScore> questionScores = await _questionScoreRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListQuestionScoreListItemDto> response = _mapper.Map<GetListResponse<GetListQuestionScoreListItemDto>>(questionScores);
            return response;
        }
    }
}