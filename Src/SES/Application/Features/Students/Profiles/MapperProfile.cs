using Application.Features.Students.CRUD.Create;
using AutoMapper;
using Entity.Entities.Mains;

namespace Application.Features.Students.Profiles;

internal class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Student, CreateStudentDTO>().ReverseMap();
    }
}
