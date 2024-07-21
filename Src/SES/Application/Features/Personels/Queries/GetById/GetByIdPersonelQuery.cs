using Application.Features.Personels.Constants;
using Application.Features.Personels.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using MediatR;
using static Application.Features.Personels.Constants.PersonelsOperationClaims;

namespace Application.Features.Personels.Queries.GetById;

public class GetByIdPersonelQuery : IRequest<GetByIdPersonelResponse>, ISecuredRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetByIdPersonelQueryHandler : IRequestHandler<GetByIdPersonelQuery, GetByIdPersonelResponse>
    {
        private readonly IMapper _mapper;
        private readonly IPersonelRepository _personelRepository;
        private readonly PersonelBusinessRules _personelBusinessRules;

        public GetByIdPersonelQueryHandler(IMapper mapper, IPersonelRepository personelRepository, PersonelBusinessRules personelBusinessRules)
        {
            _mapper = mapper;
            _personelRepository = personelRepository;
            _personelBusinessRules = personelBusinessRules;
        }

        public async Task<GetByIdPersonelResponse> Handle(GetByIdPersonelQuery request, CancellationToken cancellationToken)
        {
            Personel? personel = await _personelRepository.GetAsync(predicate: p => p.Id == request.Id, cancellationToken: cancellationToken);
            await _personelBusinessRules.PersonelShouldExistWhenSelected(personel);

            GetByIdPersonelResponse response = _mapper.Map<GetByIdPersonelResponse>(personel);
            return response;
        }
    }
}