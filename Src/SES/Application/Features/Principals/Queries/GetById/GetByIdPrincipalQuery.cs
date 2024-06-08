using Application.Features.Principals.Constants;
using Application.Features.Principals.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using MediatR;
using static Application.Features.Principals.Constants.PrincipalsOperationClaims;

namespace Application.Features.Principals.Queries.GetById;

public class GetByIdPrincipalQuery : IRequest<GetByIdPrincipalResponse>, ISecuredRequest
{
    public int Id { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetByIdPrincipalQueryHandler : IRequestHandler<GetByIdPrincipalQuery, GetByIdPrincipalResponse>
    {
        private readonly IMapper _mapper;
        private readonly IPrincipalRepository _principalRepository;
        private readonly PrincipalBusinessRules _principalBusinessRules;

        public GetByIdPrincipalQueryHandler(IMapper mapper, IPrincipalRepository principalRepository, PrincipalBusinessRules principalBusinessRules)
        {
            _mapper = mapper;
            _principalRepository = principalRepository;
            _principalBusinessRules = principalBusinessRules;
        }

        public async Task<GetByIdPrincipalResponse> Handle(GetByIdPrincipalQuery request, CancellationToken cancellationToken)
        {
            Principal? principal = await _principalRepository.GetAsync(predicate: p => p.Id == request.Id, cancellationToken: cancellationToken);
            await _principalBusinessRules.PrincipalShouldExistWhenSelected(principal);

            GetByIdPrincipalResponse response = _mapper.Map<GetByIdPrincipalResponse>(principal);
            return response;
        }
    }
}