using Application.Features.QuestionOptions.Commands.Create;
using Application.Features.QuestionOptions.Commands.Delete;
using Application.Features.QuestionOptions.Commands.Update;
using Application.Features.QuestionOptions.Queries.GetById;
using Application.Features.QuestionOptions.Queries.GetList;
using Application.Features.QuizQuestions.Commands.Create;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.QuestionOptions.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateQuestionOptionCommand, QuestionOption>();
        CreateMap<QuestionOptionAppDto, QuestionOption>();

        CreateMap<QuestionOption, CreatedQuestionOptionResponse>();

        CreateMap<UpdateQuestionOptionCommand, QuestionOption>();
        CreateMap<QuestionOption, UpdatedQuestionOptionResponse>();

        CreateMap<DeleteQuestionOptionCommand, QuestionOption>();
        CreateMap<QuestionOption, DeletedQuestionOptionResponse>();

        CreateMap<QuestionOption, GetByIdQuestionOptionResponse>();

        CreateMap<QuestionOption, GetListQuestionOptionListItemDto>();
        CreateMap<IPaginate<QuestionOption>, GetListResponse<GetListQuestionOptionListItemDto>>();
    }
}