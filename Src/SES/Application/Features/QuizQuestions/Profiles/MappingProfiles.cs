using Application.Features.QuizQuestions.Commands.Create;
using Application.Features.QuizQuestions.Commands.Delete;
using Application.Features.QuizQuestions.Commands.Update;
using Application.Features.QuizQuestions.Queries.GetById;
using Application.Features.QuizQuestions.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.QuizQuestions.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateQuizQuestionCommand, QuizQuestion>()
            .ForMember(destinationMember: qq => qq.Question, memberOptions: opt => opt.MapFrom(qqc => qqc.Question))
            .ForMember(destinationMember: qq => qq.QuestionBody, memberOptions: opt => opt.MapFrom(qqc => qqc.QuestionBody));

        CreateMap<CreateQuizQuestionCommand, QuestionScore>()
            .ForMember(destinationMember: qs => qs.Score, memberOptions: opt => opt.MapFrom(cqqc => cqqc.Score))
            .ForMember(destinationMember: qs => qs.MaxScore, memberOptions: opt => opt.MapFrom(cqqc => cqqc.MaxScore));

        CreateMap<QuizQuestion, CreatedQuizQuestionResponse>();

        CreateMap<UpdateQuizQuestionCommand, QuizQuestion>();
        CreateMap<QuizQuestion, UpdatedQuizQuestionResponse>();

        CreateMap<DeleteQuizQuestionCommand, QuizQuestion>();
        CreateMap<QuizQuestion, DeletedQuizQuestionResponse>();

        CreateMap<QuizQuestion, GetByIdQuizQuestionResponse>();

        CreateMap<QuizQuestion, GetListQuizQuestionListItemDto>();
        CreateMap<IPaginate<QuizQuestion>, GetListResponse<GetListQuizQuestionListItemDto>>();
    }
}