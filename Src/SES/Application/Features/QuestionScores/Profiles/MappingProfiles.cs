using Application.Features.QuestionScores.Commands.Create;
using Application.Features.QuestionScores.Commands.Delete;
using Application.Features.QuestionScores.Commands.Update;
using Application.Features.QuestionScores.Queries.GetById;
using Application.Features.QuestionScores.Queries.GetList;
using AutoMapper;
using NArchitecture.Core.Application.Responses;
using Domain.Entities;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.QuestionScores.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateQuestionScoreCommand, QuestionScore>();
        CreateMap<QuestionScore, CreatedQuestionScoreResponse>();

        CreateMap<UpdateQuestionScoreCommand, QuestionScore>();
        CreateMap<QuestionScore, UpdatedQuestionScoreResponse>();

        CreateMap<DeleteQuestionScoreCommand, QuestionScore>();
        CreateMap<QuestionScore, DeletedQuestionScoreResponse>();

        CreateMap<QuestionScore, GetByIdQuestionScoreResponse>();

        CreateMap<QuestionScore, GetListQuestionScoreListItemDto>();
        CreateMap<IPaginate<QuestionScore>, GetListResponse<GetListQuestionScoreListItemDto>>();
    }
}