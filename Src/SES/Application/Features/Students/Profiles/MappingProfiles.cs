using Application.Features.Students.Commands.Create;
using Application.Features.Students.Commands.Delete;
using Application.Features.Students.Commands.MultiCreate;
using Application.Features.Students.Commands.Update;
using Application.Features.Students.Queries.GetById;
using Application.Features.Students.Queries.GetList;
using Application.Services.PdfReaderService.Dtos;
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

        CreateMap<Student, CreatedMultiStudentResponse>().ReverseMap();
        CreateMap<ICollection<Student>, ICollection<CreatedMultiStudentResponse>>().ReverseMap();

        CreateMap<UpdateStudentCommand, Student>();
        CreateMap<Student, UpdatedStudentResponse>();

        CreateMap<DeleteStudentCommand, Student>();
        CreateMap<Student, DeletedStudentResponse>();

        CreateMap<Student, GetByIdStudentResponse>();

        CreateMap<Student, GetListStudentListItemDto>();
        CreateMap<IPaginate<Student>, GetListResponse<GetListStudentListItemDto>>();
    }
}