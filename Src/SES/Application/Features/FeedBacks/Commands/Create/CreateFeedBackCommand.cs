using Application.Features.FeedBacks.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.FeedBacks.Commands.Create;

public class CreateFeedBackCommand : IRequest<CreatedFeedBackResponse>
{
    public string? UserName { get; set; }
    public required string Email { get; set; }
    public required string PageUrl { get; set; }
    public required string Message { get; set; }

    public class CreateFeedBackCommandHandler : IRequestHandler<CreateFeedBackCommand, CreatedFeedBackResponse>
    {
        private readonly IMapper _mapper;
        private readonly IFeedBackRepository _feedBackRepository;
        private readonly FeedBackBusinessRules _feedBackBusinessRules;

        public CreateFeedBackCommandHandler(IMapper mapper, IFeedBackRepository feedBackRepository,
                                         FeedBackBusinessRules feedBackBusinessRules)
        {
            _mapper = mapper;
            _feedBackRepository = feedBackRepository;
            _feedBackBusinessRules = feedBackBusinessRules;
        }

        public async Task<CreatedFeedBackResponse> Handle(CreateFeedBackCommand request, CancellationToken cancellationToken)
        {
            FeedBack feedBack = _mapper.Map<FeedBack>(request);
            feedBack.SubmittedAt = DateTime.UtcNow;

            await _feedBackRepository.AddAsync(feedBack);

            CreatedFeedBackResponse response = new() { Success = true };
            return response;
        }
    }
}