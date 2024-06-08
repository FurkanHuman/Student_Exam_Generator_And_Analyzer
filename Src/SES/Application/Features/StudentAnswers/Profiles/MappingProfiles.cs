using Application.Features.StudentAnswers.Commands.Create;
using Application.Features.StudentAnswers.Commands.Delete;
using Application.Features.StudentAnswers.Commands.Update;
using Application.Features.StudentAnswers.Queries.GetById;
using Application.Features.StudentAnswers.Queries.GetList;
using AutoMapper;
using NArchitecture.Core.Application.Responses;
using Domain.Entities;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.StudentAnswers.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateStudentAnswerCommand, StudentAnswer>();
        CreateMap<StudentAnswer, CreatedStudentAnswerResponse>();

        CreateMap<UpdateStudentAnswerCommand, StudentAnswer>();
        CreateMap<StudentAnswer, UpdatedStudentAnswerResponse>();

        CreateMap<DeleteStudentAnswerCommand, StudentAnswer>();
        CreateMap<StudentAnswer, DeletedStudentAnswerResponse>();

        CreateMap<StudentAnswer, GetByIdStudentAnswerResponse>();

        CreateMap<StudentAnswer, GetListStudentAnswerListItemDto>();
        CreateMap<IPaginate<StudentAnswer>, GetListResponse<GetListStudentAnswerListItemDto>>();
    }
}