using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.ExamConfigurations.Queries.GetList;

public class GetListExamConfigurationQuery : IRequest<GetListResponse<GetListExamConfigurationListItemDto>>
{
    public PageRequest PageRequest { get; set; }

    public class GetListExamConfigurationQueryHandler : IRequestHandler<GetListExamConfigurationQuery, GetListResponse<GetListExamConfigurationListItemDto>>
    {
        private readonly IExamConfigurationRepository _examConfigurationRepository;
        private readonly IMapper _mapper;

        public GetListExamConfigurationQueryHandler(IExamConfigurationRepository examConfigurationRepository, IMapper mapper)
        {
            _examConfigurationRepository = examConfigurationRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListExamConfigurationListItemDto>> Handle(GetListExamConfigurationQuery request, CancellationToken cancellationToken)
        {
            IPaginate<ExamConfiguration> examConfigurations = await _examConfigurationRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListExamConfigurationListItemDto> response = _mapper.Map<GetListResponse<GetListExamConfigurationListItemDto>>(examConfigurations);
            return response;
        }
    }
}