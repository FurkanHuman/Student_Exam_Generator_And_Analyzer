using Application.Features.StudentExamAnswers.Commands.Create;
using Application.Features.StudentExamAnswers.Commands.Delete;
using Application.Features.StudentExamAnswers.Commands.Update;
using Application.Features.StudentExamAnswers.Queries.GetById;
using Application.Features.StudentExamAnswers.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.StudentExamAnswers.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateStudentExamAnswerCommand, StudentExamAnswer>()
            .ForMember(dest => dest.StudentAnswers,
                opt => opt.MapFrom(src => src.StudentQuestionAnswers));
        CreateMap<StudentExamAnswer, CreatedStudentExamAnswerResponse>();

        CreateMap<StudentQuestionAnswerDto, StudentAnswer>();

        CreateMap<UpdateStudentExamAnswerCommand, StudentExamAnswer>();
        CreateMap<StudentExamAnswer, UpdatedStudentExamAnswerResponse>();

        CreateMap<DeleteStudentExamAnswerCommand, StudentExamAnswer>();
        CreateMap<StudentExamAnswer, DeletedStudentExamAnswerResponse>();

        CreateMap<StudentExamAnswer, GetByIdStudentExamAnswerResponse>();

        CreateMap<StudentExamAnswer, GetListStudentExamAnswerListItemDto>();
        CreateMap<IPaginate<StudentExamAnswer>, GetListResponse<GetListStudentExamAnswerListItemDto>>();
    }
}