using Application.Features.LearningAreas.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;

namespace Application.Features.LearningAreas.Commands.Update;

public class UpdateLearningAreaCommand : IRequest<UpdatedLearningAreaResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int SubLearningAreaId { get; set; }



    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetLearningAreas"];

    public class UpdateLearningAreaCommandHandler : IRequestHandler<UpdateLearningAreaCommand, UpdatedLearningAreaResponse>
    {
        private readonly IMapper _mapper;
        private readonly ILearningAreaRepository _learningAreaRepository;
        private readonly LearningAreaBusinessRules _learningAreaBusinessRules;

        public UpdateLearningAreaCommandHandler(IMapper mapper, ILearningAreaRepository learningAreaRepository,
                                         LearningAreaBusinessRules learningAreaBusinessRules)
        {
            _mapper = mapper;
            _learningAreaRepository = learningAreaRepository;
            _learningAreaBusinessRules = learningAreaBusinessRules;
        }

        public async Task<UpdatedLearningAreaResponse> Handle(UpdateLearningAreaCommand request, CancellationToken cancellationToken)
        {
            LearningArea? learningArea = await _learningAreaRepository.GetAsync(predicate: la => la.Id == request.Id, cancellationToken: cancellationToken);
            await _learningAreaBusinessRules.LearningAreaShouldExistWhenSelected(learningArea);
            learningArea = _mapper.Map(request, learningArea);

            await _learningAreaRepository.UpdateAsync(learningArea!);

            UpdatedLearningAreaResponse response = _mapper.Map<UpdatedLearningAreaResponse>(learningArea);
            return response;
        }
    }
}