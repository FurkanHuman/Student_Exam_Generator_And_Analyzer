using Application.Features.Schools.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using static Application.Features.Schools.Constants.SchoolsOperationClaims;

namespace Application.Features.Schools.Queries.GetById;

public class GetByIdSchoolQuery : IRequest<GetByIdSchoolResponse>, ISecuredRequest
{
    public int Id { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetByIdSchoolQueryHandler : IRequestHandler<GetByIdSchoolQuery, GetByIdSchoolResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISchoolRepository _schoolRepository;
        private readonly SchoolBusinessRules _schoolBusinessRules;

        public GetByIdSchoolQueryHandler(IMapper mapper, ISchoolRepository schoolRepository, SchoolBusinessRules schoolBusinessRules)
        {
            _mapper = mapper;
            _schoolRepository = schoolRepository;
            _schoolBusinessRules = schoolBusinessRules;
        }

        public async Task<GetByIdSchoolResponse> Handle(GetByIdSchoolQuery request, CancellationToken cancellationToken)
        {
            School? school = await _schoolRepository.GetAsync(predicate: s => s.Id == request.Id, cancellationToken: cancellationToken);
            await _schoolBusinessRules.SchoolShouldExistWhenSelected(school);

            GetByIdSchoolResponse response = _mapper.Map<GetByIdSchoolResponse>(school);
            return response;
        }
    }
}