using Application.Features.SemesterFeature.CRUD;
using Application.Features.Students.CRUD.Create;
using AutoMapper;
using Entity.Entities.Infos;

namespace Application.Features.SemesterFeature.Profiles;

internal class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Semester, CreateSemesterDTO>().ReverseMap();
        CreateMap<Semester, RequestSemesterDTO>().ReverseMap();
    }
}
