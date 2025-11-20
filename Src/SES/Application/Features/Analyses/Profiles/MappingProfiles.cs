using Application.Features.Analyses.Commands.Create;
using Application.Features.Analyses.Commands.Delete;
using Application.Features.Analyses.Commands.Update;
using Application.Features.Analyses.Queries.GetById;
using Application.Features.Analyses.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Analyses.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateAnalysisCommand, Analysis>();
        CreateMap<Analysis, CreatedAnalysisResponse>();

        CreateMap<UpdateAnalysisCommand, Analysis>();
        CreateMap<Analysis, UpdatedAnalysisResponse>();

        CreateMap<DeleteAnalysisCommand, Analysis>();
        CreateMap<Analysis, DeletedAnalysisResponse>();

        CreateMap<Analysis, GetByIdAnalysisResponse>();

        CreateMap<Analysis, GetListAnalysisListItemDto>()
            .ForMember(destinationMember: ald => ald.SemesterName, memberOptions: mo => mo.MapFrom(a => a.Semester.Name))
            .ForMember(destinationMember: ald => ald.SemesterStartDate, memberOptions: mo => mo.MapFrom(a => a.Semester.BeginSemesterDate))
            .ForMember(destinationMember: ald => ald.SemesterEndDate, memberOptions: mo => mo.MapFrom(a => a.Semester.EndSemesterDate))
            .ForMember(destinationMember: ald => ald.LessonName, memberOptions: mo => mo.MapFrom(a => a.Lesson.LessonName));
        CreateMap<IPaginate<Analysis>, GetListResponse<GetListAnalysisListItemDto>>();
    }
}