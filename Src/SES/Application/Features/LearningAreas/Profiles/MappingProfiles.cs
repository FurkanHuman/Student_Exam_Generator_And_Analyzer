using Application.Features.LearningAreas.Commands.Create;
using Application.Features.LearningAreas.Commands.Delete;
using Application.Features.LearningAreas.Commands.Update;
using Application.Features.LearningAreas.Queries.GetById;
using Application.Features.LearningAreas.Queries.GetList;
using AutoMapper;
using NArchitecture.Core.Application.Responses;
using Domain.Entities;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.LearningAreas.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateLearningAreaCommand, LearningArea>();
        CreateMap<LearningArea, CreatedLearningAreaResponse>();

        CreateMap<UpdateLearningAreaCommand, LearningArea>();
        CreateMap<LearningArea, UpdatedLearningAreaResponse>();

        CreateMap<DeleteLearningAreaCommand, LearningArea>();
        CreateMap<LearningArea, DeletedLearningAreaResponse>();

        CreateMap<LearningArea, GetByIdLearningAreaResponse>();

        CreateMap<LearningArea, GetListLearningAreaListItemDto>();
        CreateMap<IPaginate<LearningArea>, GetListResponse<GetListLearningAreaListItemDto>>();
    }
}