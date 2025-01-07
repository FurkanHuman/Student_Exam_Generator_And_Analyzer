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

namespace Application.Features.LearningAreas.Commands.Delete;

public class DeleteLearningAreaCommand : IRequest<DeletedLearningAreaResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }

    

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetLearningAreas"];

    public class DeleteLearningAreaCommandHandler : IRequestHandler<DeleteLearningAreaCommand, DeletedLearningAreaResponse>
    {
        private readonly IMapper _mapper;
        private readonly ILearningAreaRepository _learningAreaRepository;
        private readonly LearningAreaBusinessRules _learningAreaBusinessRules;

        public DeleteLearningAreaCommandHandler(IMapper mapper, ILearningAreaRepository learningAreaRepository,
                                         LearningAreaBusinessRules learningAreaBusinessRules)
        {
            _mapper = mapper;
            _learningAreaRepository = learningAreaRepository;
            _learningAreaBusinessRules = learningAreaBusinessRules;
        }

        public async Task<DeletedLearningAreaResponse> Handle(DeleteLearningAreaCommand request, CancellationToken cancellationToken)
        {
            LearningArea? learningArea = await _learningAreaRepository.GetAsync(predicate: la => la.Id == request.Id, cancellationToken: cancellationToken);
            await _learningAreaBusinessRules.LearningAreaShouldExistWhenSelected(learningArea);

            await _learningAreaRepository.DeleteAsync(learningArea!);

            DeletedLearningAreaResponse response = _mapper.Map<DeletedLearningAreaResponse>(learningArea);
            return response;
        }
    }
}