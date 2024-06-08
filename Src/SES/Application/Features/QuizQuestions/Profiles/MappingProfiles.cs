using Application.Features.QuizQuestions.Commands.Create;
using Application.Features.QuizQuestions.Commands.Delete;
using Application.Features.QuizQuestions.Commands.Update;
using Application.Features.QuizQuestions.Queries.GetById;
using Application.Features.QuizQuestions.Queries.GetList;
using AutoMapper;
using NArchitecture.Core.Application.Responses;
using Domain.Entities;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.QuizQuestions.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateQuizQuestionCommand, QuizQuestion>();
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