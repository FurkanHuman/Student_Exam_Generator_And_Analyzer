using Application.Features.FeedBacks.Constants;
using Application.Features.FeedBacks.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.FeedBacks.Commands.Delete;

public class DeleteFeedBackCommand : IRequest<DeletedFeedBackResponse>
{
    public Guid Id { get; set; }

    public class DeleteFeedBackCommandHandler : IRequestHandler<DeleteFeedBackCommand, DeletedFeedBackResponse>
    {
        private readonly IMapper _mapper;
        private readonly IFeedBackRepository _feedBackRepository;
        private readonly FeedBackBusinessRules _feedBackBusinessRules;

        public DeleteFeedBackCommandHandler(IMapper mapper, IFeedBackRepository feedBackRepository,
                                         FeedBackBusinessRules feedBackBusinessRules)
        {
            _mapper = mapper;
            _feedBackRepository = feedBackRepository;
            _feedBackBusinessRules = feedBackBusinessRules;
        }

        public async Task<DeletedFeedBackResponse> Handle(DeleteFeedBackCommand request, CancellationToken cancellationToken)
        {
            FeedBack? feedBack = await _feedBackRepository.GetAsync(predicate: fb => fb.Id == request.Id, cancellationToken: cancellationToken);
            await _feedBackBusinessRules.FeedBackShouldExistWhenSelected(feedBack);

            await _feedBackRepository.DeleteAsync(feedBack!);

            DeletedFeedBackResponse response = _mapper.Map<DeletedFeedBackResponse>(feedBack);
            return response;
        }
    }
}