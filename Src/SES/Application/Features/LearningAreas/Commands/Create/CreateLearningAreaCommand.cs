using Application.Features.LearningAreas.Constants;
using Application.Features.LearningAreas.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.LearningAreas.Constants.LearningAreasOperationClaims;

namespace Application.Features.LearningAreas.Commands.Create;

public class CreateLearningAreaCommand : IRequest<CreatedLearningAreaResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public required string Name { get; set; }
    public required int SubLearningAreaId { get; set; }

    

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetLearningAreas"];

    public class CreateLearningAreaCommandHandler : IRequestHandler<CreateLearningAreaCommand, CreatedLearningAreaResponse>
    {
        private readonly IMapper _mapper;
        private readonly ILearningAreaRepository _learningAreaRepository;
        private readonly LearningAreaBusinessRules _learningAreaBusinessRules;

        public CreateLearningAreaCommandHandler(IMapper mapper, ILearningAreaRepository learningAreaRepository,
                                         LearningAreaBusinessRules learningAreaBusinessRules)
        {
            _mapper = mapper;
            _learningAreaRepository = learningAreaRepository;
            _learningAreaBusinessRules = learningAreaBusinessRules;
        }

        public async Task<CreatedLearningAreaResponse> Handle(CreateLearningAreaCommand request, CancellationToken cancellationToken)
        {
            LearningArea learningArea = _mapper.Map<LearningArea>(request);

            await _learningAreaRepository.AddAsync(learningArea);

            CreatedLearningAreaResponse response = _mapper.Map<CreatedLearningAreaResponse>(learningArea);
            return response;
        }
    }
}