using Application.Features.Semesters.Commands.Create;
using Application.Features.Semesters.Commands.Delete;
using Application.Features.Semesters.Commands.Update;
using Application.Features.Semesters.Queries.GetById;
using Application.Features.Semesters.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Semesters.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateSemesterCommand, Semester>();
        CreateMap<Semester, CreatedSemesterResponse>();

        CreateMap<UpdateSemesterCommand, Semester>();
        CreateMap<Semester, UpdatedSemesterResponse>();

        CreateMap<DeleteSemesterCommand, Semester>();
        CreateMap<Semester, DeletedSemesterResponse>();

        CreateMap<Semester, GetByIdSemesterResponse>();

        CreateMap<Semester, GetListSemesterListItemDto>();
        CreateMap<IPaginate<Semester>, GetListResponse<GetListSemesterListItemDto>>();
    }
}