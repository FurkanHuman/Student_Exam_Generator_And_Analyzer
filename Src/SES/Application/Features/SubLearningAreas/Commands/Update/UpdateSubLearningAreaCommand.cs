using Application.Features.SubLearningAreas.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;

namespace Application.Features.SubLearningAreas.Commands.Update;

public class UpdateSubLearningAreaCommand : IRequest<UpdatedSubLearningAreaResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int BenefitId { get; set; }



    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetSubLearningAreas"];

    public class UpdateSubLearningAreaCommandHandler : IRequestHandler<UpdateSubLearningAreaCommand, UpdatedSubLearningAreaResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISubLearningAreaRepository _subLearningAreaRepository;
        private readonly SubLearningAreaBusinessRules _subLearningAreaBusinessRules;

        public UpdateSubLearningAreaCommandHandler(IMapper mapper, ISubLearningAreaRepository subLearningAreaRepository,
                                         SubLearningAreaBusinessRules subLearningAreaBusinessRules)
        {
            _mapper = mapper;
            _subLearningAreaRepository = subLearningAreaRepository;
            _subLearningAreaBusinessRules = subLearningAreaBusinessRules;
        }

        public async Task<UpdatedSubLearningAreaResponse> Handle(UpdateSubLearningAreaCommand request, CancellationToken cancellationToken)
        {
            SubLearningArea? subLearningArea = await _subLearningAreaRepository.GetAsync(predicate: sla => sla.Id == request.Id, cancellationToken: cancellationToken);
            await _subLearningAreaBusinessRules.SubLearningAreaShouldExistWhenSelected(subLearningArea);
            subLearningArea = _mapper.Map(request, subLearningArea);

            await _subLearningAreaRepository.UpdateAsync(subLearningArea!);

            UpdatedSubLearningAreaResponse response = _mapper.Map<UpdatedSubLearningAreaResponse>(subLearningArea);
            return response;
        }
    }
}