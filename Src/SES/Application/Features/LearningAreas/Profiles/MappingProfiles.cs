using Application.Features.LearningAreas.Commands.Create;
using Application.Features.LearningAreas.Commands.Delete;
using Application.Features.LearningAreas.Commands.Update;
using Application.Features.LearningAreas.Queries.GetById;
using Application.Features.LearningAreas.Queries.GetList;
using Application.Services.PdfReaderService.Dtos;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.LearningAreas.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateLearningAreaCommand, LearningArea>();
        CreateMap<LearningArea, CreatedLearningAreaResponse>();

        CreateMap<LearningArea, LearningAreaDto>()
            .ForMember(destinationMember: lad => lad.LACode, memberOptions: opt => opt.MapFrom(la => la.LACode))
            .ForMember(destinationMember: lad => lad.Description, memberOptions: opt => opt.MapFrom(la => la.Description))
            .ReverseMap()
            .ForMember(destinationMember: la => la.CreatedDate, memberOptions: opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<UpdateLearningAreaCommand, LearningArea>();
        CreateMap<LearningArea, UpdatedLearningAreaResponse>();

        CreateMap<DeleteLearningAreaCommand, LearningArea>();
        CreateMap<LearningArea, DeletedLearningAreaResponse>();

        CreateMap<LearningArea, GetByIdLearningAreaResponse>();

        CreateMap<LearningArea, GetListLearningAreaListItemDto>();
        CreateMap<IPaginate<LearningArea>, GetListResponse<GetListLearningAreaListItemDto>>();
    }
}