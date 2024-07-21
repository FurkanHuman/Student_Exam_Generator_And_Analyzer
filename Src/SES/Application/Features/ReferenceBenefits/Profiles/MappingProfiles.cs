using Application.Features.ReferenceBenefits.Commands.Create;
using Application.Features.ReferenceBenefits.Commands.Delete;
using Application.Features.ReferenceBenefits.Commands.Update;
using Application.Features.ReferenceBenefits.Queries.GetById;
using Application.Features.ReferenceBenefits.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.ReferenceBenefits.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateReferenceBenefitCommand, ReferenceBenefit>();
        CreateMap<ReferenceBenefit, CreatedReferenceBenefitResponse>();

        CreateMap<UpdateReferenceBenefitCommand, ReferenceBenefit>();
        CreateMap<ReferenceBenefit, UpdatedReferenceBenefitResponse>();

        CreateMap<DeleteReferenceBenefitCommand, ReferenceBenefit>();
        CreateMap<ReferenceBenefit, DeletedReferenceBenefitResponse>();

        CreateMap<ReferenceBenefit, GetByIdReferenceBenefitResponse>();

        CreateMap<ReferenceBenefit, GetListReferenceBenefitListItemDto>();
        CreateMap<IPaginate<ReferenceBenefit>, GetListResponse<GetListReferenceBenefitListItemDto>>();
    }
}