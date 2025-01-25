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
            .ForMember(destinationMember: sld => sld.SLCode, memberOptions: opt => opt.MapFrom(sl => sl.SLACode))
            .ForMember(destinationMember: sld => sld.Description, memberOptions: opt => opt.MapFrom(sl => sl.Description))
            .ReverseMap()
            .ForMember(destinationMember: sl => sl.CreatedDate, memberOptions: opt => opt.MapFrom(_ => DateTime.UtcNow)); ;

        CreateMap<UpdateSubLearningAreaCommand, SubLearningArea>();
        CreateMap<SubLearningArea, UpdatedSubLearningAreaResponse>();

        CreateMap<DeleteSubLearningAreaCommand, SubLearningArea>();
        CreateMap<SubLearningArea, DeletedSubLearningAreaResponse>();

        CreateMap<SubLearningArea, GetByIdSubLearningAreaResponse>();

        CreateMap<SubLearningArea, GetListSubLearningAreaListItemDto>();
        CreateMap<IPaginate<SubLearningArea>, GetListResponse<GetListSubLearningAreaListItemDto>>();
    }
}