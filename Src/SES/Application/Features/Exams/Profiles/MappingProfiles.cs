using Application.Features.Exams.Commands.Create;
using Application.Features.Exams.Commands.Delete;
using Application.Features.Exams.Commands.Update;
using Application.Features.Exams.Queries.GetById;
using Application.Features.Exams.Queries.GetList;
using AutoMapper;
using NArchitecture.Core.Application.Responses;
using Domain.Entities;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Exams.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateExamCommand, Exam>();
        CreateMap<Exam, CreatedExamResponse>();

        CreateMap<UpdateExamCommand, Exam>();
        CreateMap<Exam, UpdatedExamResponse>();

        CreateMap<DeleteExamCommand, Exam>();
        CreateMap<Exam, DeletedExamResponse>();

        CreateMap<Exam, GetByIdExamResponse>();

        CreateMap<Exam, GetListExamListItemDto>();
        CreateMap<IPaginate<Exam>, GetListResponse<GetListExamListItemDto>>();
    }
}