using Application.Features.SubLearningAreas.Constants;
using Application.Features.SubLearningAreas.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.SubLearningAreas.Constants.SubLearningAreasOperationClaims;

namespace Application.Features.SubLearningAreas.Commands.Create;

public class CreateSubLearningAreaCommand : IRequest<CreatedSubLearningAreaResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public required string Name { get; set; }
    public required int BenefitId { get; set; }

    public string[] Roles => [Admin, Write, SubLearningAreasOperationClaims.Create];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetSubLearningAreas"];

    public class CreateSubLearningAreaCommandHandler : IRequestHandler<CreateSubLearningAreaCommand, CreatedSubLearningAreaResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISubLearningAreaRepository _subLearningAreaRepository;
        private readonly SubLearningAreaBusinessRules _subLearningAreaBusinessRules;

        public CreateSubLearningAreaCommandHandler(IMapper mapper, ISubLearningAreaRepository subLearningAreaRepository,
                                         SubLearningAreaBusinessRules subLearningAreaBusinessRules)
        {
            _mapper = mapper;
            _subLearningAreaRepository = subLearningAreaRepository;
            _subLearningAreaBusinessRules = subLearningAreaBusinessRules;
        }

        public async Task<CreatedSubLearningAreaResponse> Handle(CreateSubLearningAreaCommand request, CancellationToken cancellationToken)
        {
            SubLearningArea subLearningArea = _mapper.Map<SubLearningArea>(request);

            await _subLearningAreaRepository.AddAsync(subLearningArea);

            CreatedSubLearningAreaResponse response = _mapper.Map<CreatedSubLearningAreaResponse>(subLearningArea);
            return response;
        }
    }
}