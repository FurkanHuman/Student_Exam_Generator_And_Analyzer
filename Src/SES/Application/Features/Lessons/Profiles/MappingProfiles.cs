using Application.Features.Lessons.Commands.Create;
using Application.Features.Lessons.Commands.Delete;
using Application.Features.Lessons.Commands.Update;
using Application.Features.Lessons.Queries.GetById;
using Application.Features.Lessons.Queries.GetList;
using AutoMapper;
using NArchitecture.Core.Application.Responses;
using Domain.Entities;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Lessons.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateLessonCommand, Lesson>();
        CreateMap<Lesson, CreatedLessonResponse>();

        CreateMap<UpdateLessonCommand, Lesson>();
        CreateMap<Lesson, UpdatedLessonResponse>();

        CreateMap<DeleteLessonCommand, Lesson>();
        CreateMap<Lesson, DeletedLessonResponse>();

        CreateMap<Lesson, GetByIdLessonResponse>();

        CreateMap<Lesson, GetListLessonListItemDto>();
        CreateMap<IPaginate<Lesson>, GetListResponse<GetListLessonListItemDto>>();
    }
}