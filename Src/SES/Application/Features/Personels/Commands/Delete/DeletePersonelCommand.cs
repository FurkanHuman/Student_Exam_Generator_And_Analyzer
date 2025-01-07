using Application.Features.Personels.Constants;
using Application.Features.Personels.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Personels.Constants.PersonelsOperationClaims;

namespace Application.Features.Personels.Commands.Delete;

public class DeletePersonelCommand : IRequest<DeletedPersonelResponse>, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }

    

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetPersonels"];

    public class DeletePersonelCommandHandler : IRequestHandler<DeletePersonelCommand, DeletedPersonelResponse>
    {
        private readonly IMapper _mapper;
        private readonly IPersonelRepository _personelRepository;
        private readonly PersonelBusinessRules _personelBusinessRules;

        public DeletePersonelCommandHandler(IMapper mapper, IPersonelRepository personelRepository,
                                         PersonelBusinessRules personelBusinessRules)
        {
            _mapper = mapper;
            _personelRepository = personelRepository;
            _personelBusinessRules = personelBusinessRules;
        }

        public async Task<DeletedPersonelResponse> Handle(DeletePersonelCommand request, CancellationToken cancellationToken)
        {
            Personel? personel = await _personelRepository.GetAsync(predicate: p => p.Id == request.Id, cancellationToken: cancellationToken);
            await _personelBusinessRules.PersonelShouldExistWhenSelected(personel);

            await _personelRepository.DeleteAsync(personel!);

            DeletedPersonelResponse response = _mapper.Map<DeletedPersonelResponse>(personel);
            return response;
        }
    }
}