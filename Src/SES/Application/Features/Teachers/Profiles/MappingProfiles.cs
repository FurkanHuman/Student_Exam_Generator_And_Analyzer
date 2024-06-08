using Application.Features.Teachers.Commands.Create;
using Application.Features.Teachers.Commands.Delete;
using Application.Features.Teachers.Commands.Update;
using Application.Features.Teachers.Queries.GetById;
using Application.Features.Teachers.Queries.GetList;
using AutoMapper;
using NArchitecture.Core.Application.Responses;
using Domain.Entities;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Teachers.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateTeacherCommand, Teacher>();
        CreateMap<Teacher, CreatedTeacherResponse>();

        CreateMap<UpdateTeacherCommand, Teacher>();
        CreateMap<Teacher, UpdatedTeacherResponse>();

        CreateMap<DeleteTeacherCommand, Teacher>();
        CreateMap<Teacher, DeletedTeacherResponse>();

        CreateMap<Teacher, GetByIdTeacherResponse>();

        CreateMap<Teacher, GetListTeacherListItemDto>();
        CreateMap<IPaginate<Teacher>, GetListResponse<GetListTeacherListItemDto>>();
    }
}