using Application.Features.SubLearningAreas.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using static Application.Features.SubLearningAreas.Constants.SubLearningAreasOperationClaims;

namespace Application.Features.SubLearningAreas.Queries.GetById;

public class GetByIdSubLearningAreaQuery : IRequest<GetByIdSubLearningAreaResponse>, ISecuredRequest
{
    public int Id { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetByIdSubLearningAreaQueryHandler : IRequestHandler<GetByIdSubLearningAreaQuery, GetByIdSubLearningAreaResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISubLearningAreaRepository _subLearningAreaRepository;
        private readonly SubLearningAreaBusinessRules _subLearningAreaBusinessRules;

        public GetByIdSubLearningAreaQueryHandler(IMapper mapper, ISubLearningAreaRepository subLearningAreaRepository, SubLearningAreaBusinessRules subLearningAreaBusinessRules)
        {
            _mapper = mapper;
            _subLearningAreaRepository = subLearningAreaRepository;
            _subLearningAreaBusinessRules = subLearningAreaBusinessRules;
        }

        public async Task<GetByIdSubLearningAreaResponse> Handle(GetByIdSubLearningAreaQuery request, CancellationToken cancellationToken)
        {
            SubLearningArea? subLearningArea = await _subLearningAreaRepository.GetAsync(predicate: sla => sla.Id == request.Id, cancellationToken: cancellationToken);
            await _subLearningAreaBusinessRules.SubLearningAreaShouldExistWhenSelected(subLearningArea);

            GetByIdSubLearningAreaResponse response = _mapper.Map<GetByIdSubLearningAreaResponse>(subLearningArea);
            return response;
        }
    }
}