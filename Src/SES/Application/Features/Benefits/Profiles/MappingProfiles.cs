using Application.Features.Benefits.Commands.Create;
using Application.Features.Benefits.Commands.Delete;
using Application.Features.Benefits.Commands.MultiCreate;
using Application.Features.Benefits.Commands.Update;
using Application.Features.Benefits.Queries.GetById;
using Application.Features.Benefits.Queries.GetList;
using Application.Services.PdfReaderService.Dtos;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Benefits.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<MultiCreateBenefitCommand, Benefit>();
        CreateMap<Benefit, CreatedBenefitResponse>();

        CreateMap<Benefit, BenefitDto>()
            .ForMember(destinationMember: bd => bd.BCode, memberOptions: opt => opt.MapFrom(b => b.BenefitCode))
            .ForMember(destinationMember: bd => bd.Description, memberOptions: opt => opt.MapFrom(b => b.Description))
            .ReverseMap()
            .ForMember(destinationMember: b => b.CreatedDate, memberOptions: opt => opt.MapFrom(_ => DateTime.UtcNow));


        CreateMap<UpdateBenefitCommand, Benefit>();
        CreateMap<Benefit, UpdatedBenefitResponse>();

        CreateMap<DeleteBenefitCommand, Benefit>();
        CreateMap<Benefit, DeletedBenefitResponse>();

        CreateMap<Benefit, GetByIdBenefitResponse>();

        CreateMap<Benefit, GetListBenefitListItemDto>();
        CreateMap<IPaginate<Benefit>, GetListResponse<GetListBenefitListItemDto>>();
    }
}