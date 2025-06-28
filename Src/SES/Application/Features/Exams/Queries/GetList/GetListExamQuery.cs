using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Exams.Queries.GetList;

public class GetListExamQuery : IRequest<GetListResponse<GetListExamListItemDto>>, ICachableRequest
{
    public PageRequest PageRequest { get; set; }



    public bool BypassCache { get; }
    public string? CacheKey => $"GetListExams({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetExams";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListExamQueryHandler : IRequestHandler<GetListExamQuery, GetListResponse<GetListExamListItemDto>>
    {
        private readonly IExamRepository _examRepository;
        private readonly IMapper _mapper;

        public GetListExamQueryHandler(IExamRepository examRepository, IMapper mapper)
        {
            _examRepository = examRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListExamListItemDto>> Handle(GetListExamQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Exam> exams = await _examRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListExamListItemDto> response = _mapper.Map<GetListResponse<GetListExamListItemDto>>(exams);
            return response;
        }
    }
}