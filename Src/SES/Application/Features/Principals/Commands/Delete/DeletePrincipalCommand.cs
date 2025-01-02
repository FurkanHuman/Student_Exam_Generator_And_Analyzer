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

namespace Application.Features.Principals.Commands.Delete;

public class DeletePrincipalCommand : IRequest<DeletedPrincipalResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }

    

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetPrincipals"];

    public class DeletePrincipalCommandHandler : IRequestHandler<DeletePrincipalCommand, DeletedPrincipalResponse>
    {
        private readonly IMapper _mapper;
        private readonly IPrincipalRepository _principalRepository;
        private readonly PrincipalBusinessRules _principalBusinessRules;

        public DeletePrincipalCommandHandler(IMapper mapper, IPrincipalRepository principalRepository,
                                         PrincipalBusinessRules principalBusinessRules)
        {
            _mapper = mapper;
            _principalRepository = principalRepository;
            _principalBusinessRules = principalBusinessRules;
        }

        public async Task<DeletedPrincipalResponse> Handle(DeletePrincipalCommand request, CancellationToken cancellationToken)
        {
            Principal? principal = await _principalRepository.GetAsync(predicate: p => p.Id == request.Id, cancellationToken: cancellationToken);
            await _principalBusinessRules.PrincipalShouldExistWhenSelected(principal);

            await _principalRepository.DeleteAsync(principal!);

            DeletedPrincipalResponse response = _mapper.Map<DeletedPrincipalResponse>(principal);
            return response;
        }
    }
}