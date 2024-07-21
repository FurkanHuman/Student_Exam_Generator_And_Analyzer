using Application.Features.Personels.Constants;
using Application.Features.Personels.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.Personels.Constants.PersonelsOperationClaims;

namespace Application.Features.Personels.Commands.Create;

public class CreatePersonelCommand : IRequest<CreatedPersonelResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public required Guid UserId { get; set; }
    public required string Name { get; set; }
    public required string SurName { get; set; }
    public required DateOnly BirthDate { get; set; }

    public string[] Roles => [Admin, Write, PersonelsOperationClaims.Create];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetPersonels"];

    public class CreatePersonelCommandHandler : IRequestHandler<CreatePersonelCommand, CreatedPersonelResponse>
    {
        private readonly IMapper _mapper;
        private readonly IPersonelRepository _personelRepository;
        private readonly PersonelBusinessRules _personelBusinessRules;

        public CreatePersonelCommandHandler(IMapper mapper, IPersonelRepository personelRepository,
                                         PersonelBusinessRules personelBusinessRules)
        {
            _mapper = mapper;
            _personelRepository = personelRepository;
            _personelBusinessRules = personelBusinessRules;
        }

        public async Task<CreatedPersonelResponse> Handle(CreatePersonelCommand request, CancellationToken cancellationToken)
        {
            Personel personel = _mapper.Map<Personel>(request);

            await _personelRepository.AddAsync(personel);

            CreatedPersonelResponse response = _mapper.Map<CreatedPersonelResponse>(personel);
            return response;
        }
    }
}