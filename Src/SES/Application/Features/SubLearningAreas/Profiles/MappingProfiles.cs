using Application.Features.SubLearningAreas.Commands.Create;
using Application.Features.SubLearningAreas.Commands.Delete;
using Application.Features.SubLearningAreas.Commands.Update;
using Application.Features.SubLearningAreas.Queries.GetById;
using Application.Features.SubLearningAreas.Queries.GetList;
using Application.Services.PdfReaderService.Dtos;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.SubLearningAreas.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateSubLearningAreaCommand, SubLearningArea>();
        CreateMap<SubLearningArea, CreatedSubLearningAreaResponse>();

        CreateMap<SubLearningArea, SubLearningDto>()
            .ForMember(destinationMember: sl => sl.SLCode, memberOptions: opt => opt.MapFrom(sld => sld.SLACode))
            .ForMember(destinationMember: sl => sl.Description, memberOptions: opt => opt.MapFrom(sld => sld.Description))
            .ReverseMap();


        CreateMap<UpdateSubLearningAreaCommand, SubLearningArea>();
        CreateMap<SubLearningArea, UpdatedSubLearningAreaResponse>();

        CreateMap<DeleteSubLearningAreaCommand, SubLearningArea>();
        CreateMap<SubLearningArea, DeletedSubLearningAreaResponse>();

        CreateMap<SubLearningArea, GetByIdSubLearningAreaResponse>();

        CreateMap<SubLearningArea, GetListSubLearningAreaListItemDto>();
        CreateMap<IPaginate<SubLearningArea>, GetListResponse<GetListSubLearningAreaListItemDto>>();
    }
}