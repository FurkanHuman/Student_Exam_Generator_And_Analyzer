using Application.Features.StudentClasses.Commands.Create;
using Application.Features.StudentClasses.Commands.Delete;
using Application.Features.StudentClasses.Commands.Update;
using Application.Features.StudentClasses.Queries.GetById;
using Application.Features.StudentClasses.Queries.GetClassesByClassAge;
using Application.Features.StudentClasses.Queries.GetClassesBySemesterId;
using Application.Features.StudentClasses.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.StudentClasses.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateStudentClassCommand, StudentClass>();
        CreateMap<StudentClass, CreatedStudentClassResponse>();

        CreateMap<UpdateStudentClassCommand, StudentClass>();
        CreateMap<StudentClass, UpdatedStudentClassResponse>();

        CreateMap<DeleteStudentClassCommand, StudentClass>();
        CreateMap<StudentClass, DeletedStudentClassResponse>();

        CreateMap<StudentClass, GetByIdStudentClassResponse>();

        CreateMap<StudentClass, GetListStudentClassListItemDto>();
        CreateMap<IPaginate<StudentClass>, GetListResponse<GetListStudentClassListItemDto>>();

        CreateMap<StudentClass, GetClassesByClassAgeResponse>();
        CreateMap<IPaginate<StudentClass>, GetListResponse<GetClassesByClassAgeResponse>>();

        CreateMap<StudentClass, GetClassesBySemesterIdResponse>();
        CreateMap<IPaginate<StudentClass>, GetListResponse<GetClassesBySemesterIdResponse>>();
    }
}