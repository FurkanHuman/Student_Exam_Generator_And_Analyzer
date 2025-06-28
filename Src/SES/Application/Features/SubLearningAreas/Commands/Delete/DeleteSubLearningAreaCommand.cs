using Application.Features.SubLearningAreas.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;

namespace Application.Features.SubLearningAreas.Commands.Delete;

public class DeleteSubLearningAreaCommand : IRequest<DeletedSubLearningAreaResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }



    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetSubLearningAreas"];

    public class DeleteSubLearningAreaCommandHandler : IRequestHandler<DeleteSubLearningAreaCommand, DeletedSubLearningAreaResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISubLearningAreaRepository _subLearningAreaRepository;
        private readonly SubLearningAreaBusinessRules _subLearningAreaBusinessRules;

        public DeleteSubLearningAreaCommandHandler(IMapper mapper, ISubLearningAreaRepository subLearningAreaRepository,
                                         SubLearningAreaBusinessRules subLearningAreaBusinessRules)
        {
            _mapper = mapper;
            _subLearningAreaRepository = subLearningAreaRepository;
            _subLearningAreaBusinessRules = subLearningAreaBusinessRules;
        }

        public async Task<DeletedSubLearningAreaResponse> Handle(DeleteSubLearningAreaCommand request, CancellationToken cancellationToken)
        {
            SubLearningArea? subLearningArea = await _subLearningAreaRepository.GetAsync(predicate: sla => sla.Id == request.Id, cancellationToken: cancellationToken);
            await _subLearningAreaBusinessRules.SubLearningAreaShouldExistWhenSelected(subLearningArea);

            await _subLearningAreaRepository.DeleteAsync(subLearningArea!);

            DeletedSubLearningAreaResponse response = _mapper.Map<DeletedSubLearningAreaResponse>(subLearningArea);
            return response;
        }
    }
}