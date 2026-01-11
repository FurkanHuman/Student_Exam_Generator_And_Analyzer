using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using MediatR;

namespace Application.Features.FeedBacks.Queries.GetList;

public class GetListFeedBackQuery : IRequest<GetListResponse<GetListFeedBackListItemDto>>
{
    public PageRequest PageRequest { get; set; }

    public class GetListFeedBackQueryHandler : IRequestHandler<GetListFeedBackQuery, GetListResponse<GetListFeedBackListItemDto>>
    {
        private readonly IFeedBackRepository _feedBackRepository;
        private readonly IMapper _mapper;

        public GetListFeedBackQueryHandler(IFeedBackRepository feedBackRepository, IMapper mapper)
        {
            _feedBackRepository = feedBackRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListFeedBackListItemDto>> Handle(GetListFeedBackQuery request, CancellationToken cancellationToken)
        {
            IPaginate<FeedBack> feedBacks = await _feedBackRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize, 
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListFeedBackListItemDto> response = _mapper.Map<GetListResponse<GetListFeedBackListItemDto>>(feedBacks);
            return response;
        }
    }
}