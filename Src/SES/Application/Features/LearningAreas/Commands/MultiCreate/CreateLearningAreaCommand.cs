using Application.Features.LearningAreas.Constants;
using Application.Features.LearningAreas.Rules;
using Application.Services.PdfReaderService.Dtos;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.LearningAreas.Constants.LearningAreasOperationClaims;

namespace Application.Features.LearningAreas.Commands.MultiCreate;

public class CreateLearningAreaCommand : IRequest<MultiCreatedLearningAreaResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public required ICollection<LearningAreaDto> LearningAreas { get; set; }

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetLearningAreas"];

    public class CreateLearningAreaCommandHandler : IRequestHandler<CreateLearningAreaCommand, MultiCreatedLearningAreaResponse>
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

        public async Task<MultiCreatedLearningAreaResponse> Handle(CreateLearningAreaCommand request, CancellationToken cancellationToken)
        {
           ICollection< LearningArea> learningArea = _mapper.Map<ICollection<LearningArea>>(request);

            await _learningAreaRepository.AddRangeAsync(learningArea);

            MultiCreatedLearningAreaResponse response = _mapper.Map<MultiCreatedLearningAreaResponse>(learningArea);
            return response;
        }
    }
}