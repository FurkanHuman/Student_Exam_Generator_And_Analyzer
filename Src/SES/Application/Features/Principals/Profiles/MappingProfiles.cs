using Application.Features.Principals.Commands.Create;
using Application.Features.Principals.Commands.Delete;
using Application.Features.Principals.Commands.Update;
using Application.Features.Principals.Queries.GetById;
using Application.Features.Principals.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Principals.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreatePrincipalCommand, Principal>();
        CreateMap<Principal, CreatedPrincipalResponse>();

        CreateMap<UpdatePrincipalCommand, Principal>();
        CreateMap<Principal, UpdatedPrincipalResponse>();

        CreateMap<DeletePrincipalCommand, Principal>();
        CreateMap<Principal, DeletedPrincipalResponse>();

        CreateMap<Principal, GetByIdPrincipalResponse>();

        CreateMap<Principal, GetListPrincipalListItemDto>()
            .ForMember(destinationMember: p => p.Name, memberOptions: opt => opt.MapFrom(p => p.Personel.Name))
            .ForMember(destinationMember: p => p.SurName, memberOptions: opt => opt.MapFrom(p => p.Personel.SurName));

        CreateMap<IPaginate<Principal>, GetListResponse<GetListPrincipalListItemDto>>();
    }
}