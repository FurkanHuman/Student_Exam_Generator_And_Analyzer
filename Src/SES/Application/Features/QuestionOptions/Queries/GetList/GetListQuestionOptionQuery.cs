using Application.Features.QuestionOptions.Constants;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using MediatR;
using static Application.Features.QuestionOptions.Constants.QuestionOptionsOperationClaims;

namespace Application.Features.QuestionOptions.Queries.GetList;

public class GetListQuestionOptionQuery : IRequest<GetListResponse<GetListQuestionOptionListItemDto>>, ISecuredRequest, ICachableRequest
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [Admin, Read];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListQuestionOptions({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetQuestionOptions";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListQuestionOptionQueryHandler : IRequestHandler<GetListQuestionOptionQuery, GetListResponse<GetListQuestionOptionListItemDto>>
    {
        private readonly IQuestionOptionRepository _questionOptionRepository;
        private readonly IMapper _mapper;

        public GetListQuestionOptionQueryHandler(IQuestionOptionRepository questionOptionRepository, IMapper mapper)
        {
            _questionOptionRepository = questionOptionRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListQuestionOptionListItemDto>> Handle(GetListQuestionOptionQuery request, CancellationToken cancellationToken)
        {
            IPaginate<QuestionOption> questionOptions = await _questionOptionRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize, 
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListQuestionOptionListItemDto> response = _mapper.Map<GetListResponse<GetListQuestionOptionListItemDto>>(questionOptions);
            return response;
        }
    }
}