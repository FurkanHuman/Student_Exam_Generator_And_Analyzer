using Application.Features.SubLearningAreas.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.SubLearningAreas.Queries.GetById;

public class GetByIdSubLearningAreaQuery : IRequest<GetByIdSubLearningAreaResponse>
{
    public int Id { get; set; }



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