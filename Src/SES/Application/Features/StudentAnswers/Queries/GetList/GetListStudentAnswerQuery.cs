using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using static Application.Features.StudentAnswers.Constants.StudentAnswersOperationClaims;

namespace Application.Features.StudentAnswers.Queries.GetList;

public class GetListStudentAnswerQuery : IRequest<GetListResponse<GetListStudentAnswerListItemDto>>, ICachableRequest
{
    public PageRequest PageRequest { get; set; }

    

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListStudentAnswers({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetStudentAnswers";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListStudentAnswerQueryHandler : IRequestHandler<GetListStudentAnswerQuery, GetListResponse<GetListStudentAnswerListItemDto>>
    {
        private readonly IStudentAnswerRepository _studentAnswerRepository;
        private readonly IMapper _mapper;

        public GetListStudentAnswerQueryHandler(IStudentAnswerRepository studentAnswerRepository, IMapper mapper)
        {
            _studentAnswerRepository = studentAnswerRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListStudentAnswerListItemDto>> Handle(GetListStudentAnswerQuery request, CancellationToken cancellationToken)
        {
            IPaginate<StudentAnswer> studentAnswers = await _studentAnswerRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListStudentAnswerListItemDto> response = _mapper.Map<GetListResponse<GetListStudentAnswerListItemDto>>(studentAnswers);
            return response;
        }
    }
}