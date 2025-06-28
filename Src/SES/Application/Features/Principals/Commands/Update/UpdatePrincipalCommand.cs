using Application.Features.Principals.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;

namespace Application.Features.Principals.Commands.Update;

public class UpdatePrincipalCommand : IRequest<UpdatedPrincipalResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string SurName { get; set; }
    public required int SemesterId { get; set; }
    public required School School { get; set; }
    public required Semester Semester { get; set; }



    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetPrincipals"];

    public class UpdatePrincipalCommandHandler : IRequestHandler<UpdatePrincipalCommand, UpdatedPrincipalResponse>
    {
        private readonly IMapper _mapper;
        private readonly IPrincipalRepository _principalRepository;
        private readonly PrincipalBusinessRules _principalBusinessRules;

        public UpdatePrincipalCommandHandler(IMapper mapper, IPrincipalRepository principalRepository,
                                         PrincipalBusinessRules principalBusinessRules)
        {
            _mapper = mapper;
            _principalRepository = principalRepository;
            _principalBusinessRules = principalBusinessRules;
        }

        public async Task<UpdatedPrincipalResponse> Handle(UpdatePrincipalCommand request, CancellationToken cancellationToken)
        {
            Principal? principal = await _principalRepository.GetAsync(predicate: p => p.Id == request.Id, cancellationToken: cancellationToken);
            await _principalBusinessRules.PrincipalShouldExistWhenSelected(principal);
            principal = _mapper.Map(request, principal);

            await _principalRepository.UpdateAsync(principal!);

            UpdatedPrincipalResponse response = _mapper.Map<UpdatedPrincipalResponse>(principal);
            return response;
        }
    }
}