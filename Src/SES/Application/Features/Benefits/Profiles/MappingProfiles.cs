using Application.Features.Benefits.Commands.Create;
using Application.Features.Benefits.Commands.Delete;
using Application.Features.Benefits.Commands.Update;
using Application.Features.Benefits.Queries.GetById;
using Application.Features.Benefits.Queries.GetList;
using AutoMapper;
using NArchitecture.Core.Application.Responses;
using Domain.Entities;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Benefits.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateBenefitCommand, Benefit>();
        CreateMap<Benefit, CreatedBenefitResponse>();

        CreateMap<UpdateBenefitCommand, Benefit>();
        CreateMap<Benefit, UpdatedBenefitResponse>();

        CreateMap<DeleteBenefitCommand, Benefit>();
        CreateMap<Benefit, DeletedBenefitResponse>();

        CreateMap<Benefit, GetByIdBenefitResponse>();

        CreateMap<Benefit, GetListBenefitListItemDto>();
        CreateMap<IPaginate<Benefit>, GetListResponse<GetListBenefitListItemDto>>();
    }
}