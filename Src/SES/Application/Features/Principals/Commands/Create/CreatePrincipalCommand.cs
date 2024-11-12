using Application.Features.Principals.Constants;
using Application.Features.Principals.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Principals.Constants.PrincipalsOperationClaims;

namespace Application.Features.Principals.Commands.Create;

public class CreatePrincipalCommand : IRequest<CreatedPrincipalResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public required int SemesterId { get; set; }

    public required Guid PersonelId { get; set; }


    public string[] Roles => [Admin, Write, PrincipalsOperationClaims.Create];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetPrincipals"];

    public class CreatePrincipalCommandHandler : IRequestHandler<CreatePrincipalCommand, CreatedPrincipalResponse>
    {
        private readonly IMapper _mapper;
        private readonly IPrincipalRepository _principalRepository;
        private readonly PrincipalBusinessRules _principalBusinessRules;

        public CreatePrincipalCommandHandler(IMapper mapper, IPrincipalRepository principalRepository,
                                         PrincipalBusinessRules principalBusinessRules)
        {
            _mapper = mapper;
            _principalRepository = principalRepository;
            _principalBusinessRules = principalBusinessRules;
        }

        public async Task<CreatedPrincipalResponse> Handle(CreatePrincipalCommand request, CancellationToken cancellationToken)
        {
            Principal principal = _mapper.Map<Principal>(request);

            await _principalRepository.AddAsync(principal);

            CreatedPrincipalResponse response = _mapper.Map<CreatedPrincipalResponse>(principal);
            return response;
        }
    }
}