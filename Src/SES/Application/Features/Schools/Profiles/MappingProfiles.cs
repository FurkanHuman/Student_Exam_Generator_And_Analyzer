using Application.Features.Schools.Commands.Create;
using Application.Features.Schools.Commands.Delete;
using Application.Features.Schools.Commands.Update;
using Application.Features.Schools.Queries.GetById;
using Application.Features.Schools.Queries.GetList;
using AutoMapper;
using NArchitecture.Core.Application.Responses;
using Domain.Entities;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Schools.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateSchoolCommand, School>();
        CreateMap<School, CreatedSchoolResponse>();

        CreateMap<UpdateSchoolCommand, School>();
        CreateMap<School, UpdatedSchoolResponse>();

        CreateMap<DeleteSchoolCommand, School>();
        CreateMap<School, DeletedSchoolResponse>();

        CreateMap<School, GetByIdSchoolResponse>();

        CreateMap<School, GetListSchoolListItemDto>();
        CreateMap<IPaginate<School>, GetListResponse<GetListSchoolListItemDto>>();
    }
}