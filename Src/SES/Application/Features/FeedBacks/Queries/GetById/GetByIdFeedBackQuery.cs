using Application.Features.FeedBacks.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.FeedBacks.Queries.GetById;

public class GetByIdFeedBackQuery : IRequest<GetByIdFeedBackResponse>
{
    public Guid Id { get; set; }

    public class GetByIdFeedBackQueryHandler : IRequestHandler<GetByIdFeedBackQuery, GetByIdFeedBackResponse>
    {
        private readonly IMapper _mapper;
        private readonly IFeedBackRepository _feedBackRepository;
        private readonly FeedBackBusinessRules _feedBackBusinessRules;

        public GetByIdFeedBackQueryHandler(IMapper mapper, IFeedBackRepository feedBackRepository, FeedBackBusinessRules feedBackBusinessRules)
        {
            _mapper = mapper;
            _feedBackRepository = feedBackRepository;
            _feedBackBusinessRules = feedBackBusinessRules;
        }

        public async Task<GetByIdFeedBackResponse> Handle(GetByIdFeedBackQuery request, CancellationToken cancellationToken)
        {
            FeedBack? feedBack = await _feedBackRepository.GetAsync(predicate: fb => fb.Id == request.Id, cancellationToken: cancellationToken);
            await _feedBackBusinessRules.FeedBackShouldExistWhenSelected(feedBack);

            GetByIdFeedBackResponse response = _mapper.Map<GetByIdFeedBackResponse>(feedBack);
            return response;
        }
    }
}