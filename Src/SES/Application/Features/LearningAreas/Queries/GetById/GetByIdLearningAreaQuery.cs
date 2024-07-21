using Application.Features.LearningAreas.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using static Application.Features.LearningAreas.Constants.LearningAreasOperationClaims;

namespace Application.Features.LearningAreas.Queries.GetById;

public class GetByIdLearningAreaQuery : IRequest<GetByIdLearningAreaResponse>, ISecuredRequest
{
    public int Id { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetByIdLearningAreaQueryHandler : IRequestHandler<GetByIdLearningAreaQuery, GetByIdLearningAreaResponse>
    {
        private readonly IMapper _mapper;
        private readonly ILearningAreaRepository _learningAreaRepository;
        private readonly LearningAreaBusinessRules _learningAreaBusinessRules;

        public GetByIdLearningAreaQueryHandler(IMapper mapper, ILearningAreaRepository learningAreaRepository, LearningAreaBusinessRules learningAreaBusinessRules)
        {
            _mapper = mapper;
            _learningAreaRepository = learningAreaRepository;
            _learningAreaBusinessRules = learningAreaBusinessRules;
        }

        public async Task<GetByIdLearningAreaResponse> Handle(GetByIdLearningAreaQuery request, CancellationToken cancellationToken)
        {
            LearningArea? learningArea = await _learningAreaRepository.GetAsync(predicate: la => la.Id == request.Id, cancellationToken: cancellationToken);
            await _learningAreaBusinessRules.LearningAreaShouldExistWhenSelected(learningArea);

            GetByIdLearningAreaResponse response = _mapper.Map<GetByIdLearningAreaResponse>(learningArea);
            return response;
        }
    }
}