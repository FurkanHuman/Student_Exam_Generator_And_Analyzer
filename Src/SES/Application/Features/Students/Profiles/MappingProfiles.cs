using Application.Features.Students.Commands.Create;
using Application.Features.Students.Commands.Delete;
using Application.Features.Students.Commands.Update;
using Application.Features.Students.Queries.GetById;
using Application.Features.Students.Queries.GetList;
using AutoMapper;
using NArchitecture.Core.Application.Responses;
using Domain.Entities;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Students.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateStudentCommand, Student>();
        CreateMap<Student, CreatedStudentResponse>();

        CreateMap<UpdateStudentCommand, Student>();
        CreateMap<Student, UpdatedStudentResponse>();

        CreateMap<DeleteStudentCommand, Student>();
        CreateMap<Student, DeletedStudentResponse>();

        CreateMap<Student, GetByIdStudentResponse>();

        CreateMap<Student, GetListStudentListItemDto>();
        CreateMap<IPaginate<Student>, GetListResponse<GetListStudentListItemDto>>();
    }
}