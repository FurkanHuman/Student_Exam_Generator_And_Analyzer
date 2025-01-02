using Application.Features.Students.Commands.Create;
using Application.Features.Students.Commands.Delete;
using Application.Features.Students.Commands.MultiCreate;
using Application.Features.Students.Commands.Update;
using Application.Features.Students.Queries.GetById;
using Application.Features.Students.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Students.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateStudentCommand, Student>();
        CreateMap<Student, CreatedStudentResponse>();

        CreateMap<Student, CreatedMultiStudentResponse>()
            .ForMember(destinationMember: s => s.ClassAge, memberOptions: opt => opt.MapFrom(s => s.StudentClass.ClassAge))
            .ForMember(destinationMember: s => s.ClassBranch, memberOptions: opt => opt.MapFrom(s => s.StudentClass.ClassBranch));

        CreateMap<ICollection<Student>, List<CreatedMultiStudentResponse>>()
                   .ConvertUsing((src, dest, context) => src.Select(student => context.Mapper.Map<CreatedMultiStudentResponse>(student)).ToList());
        CreateMap<UpdateStudentCommand, Student>();
        CreateMap<Student, UpdatedStudentResponse>();

        CreateMap<DeleteStudentCommand, Student>();
        CreateMap<Student, DeletedStudentResponse>();

        CreateMap<Student, GetByIdStudentResponse>();

        CreateMap<Student, GetListStudentListItemDto>();
        CreateMap<IPaginate<Student>, GetListResponse<GetListStudentListItemDto>>();
    }
}