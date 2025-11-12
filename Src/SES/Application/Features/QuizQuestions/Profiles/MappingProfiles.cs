using Application.Features.QuizQuestions.Commands.Create;
using Application.Features.QuizQuestions.Commands.Delete;
using Application.Features.QuizQuestions.Commands.Update;
using Application.Features.QuizQuestions.Queries.GetById;
using Application.Features.QuizQuestions.Queries.GetByLessonId;
using Application.Features.QuizQuestions.Queries.GetList;
using Application.Features.QuizQuestions.Queries.GetQuizQuestionsByIds;
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
            .ForMember(destinationMember: qq => qq.QuestionBody, memberOptions: opt => opt.MapFrom(qqc => qqc.QuestionBody))
            .ForMember(destinationMember: qq => qq.QuestionImageURL, memberOptions: opt => opt.MapFrom(qqc => qqc.QuestionImage));

        CreateMap<CreateQuizQuestionCommand, QuestionScore>()
            .ForMember(destinationMember: qs => qs.Score, memberOptions: opt => opt.MapFrom(cqqc => cqqc.Score))
            .ForMember(destinationMember: qs => qs.MaxScore, memberOptions: opt => opt.MapFrom(cqqc => cqqc.MaxScore));

        CreateMap<QuizQuestion, CreatedQuizQuestionResponse>();

        CreateMap<UpdateQuizQuestionCommand, QuizQuestion>();
        CreateMap<QuizQuestion, UpdatedQuizQuestionResponse>();

        CreateMap<DeleteQuizQuestionCommand, QuizQuestion>();
        CreateMap<QuizQuestion, DeletedQuizQuestionResponse>();

        CreateMap<QuizQuestion, GetByIdQuizQuestionResponse>();

        CreateMap<QuizQuestion, GetListQuizQuestionListItemDto>()
            .ForMember(destinationMember: qqdto => qqdto.Score, memberOptions: opt => opt.MapFrom(qq => qq.QuestionScore.Score))
            .ForMember(destinationMember: qqdto => qqdto.MaxScore, memberOptions: opt => opt.MapFrom(qq => qq.QuestionScore.MaxScore));

        CreateMap<IPaginate<QuizQuestion>, GetListResponse<GetListQuizQuestionListItemDto>>();

        CreateMap<QuizQuestion, GetQuizQuestionsByIdsListItemDto>()
            .ForMember(destinationMember: qqdto => qqdto.Score, memberOptions: opt => opt.MapFrom(qq => qq.QuestionScore.Score))
            .ForMember(destinationMember: qqdto => qqdto.MaxScore, memberOptions: opt => opt.MapFrom(qq => qq.QuestionScore.MaxScore))
            .ForMember(destinationMember: qqdto => qqdto.Options, memberOptions: opt => opt.MapFrom(qq => qq.Options.Select(opt => new QuestionOption
            {
                Id = opt.Id,
                OptionText = opt.OptionText,
                IsCorrect = opt.IsCorrect,
                CreatedDate = opt.CreatedDate,
                UpdatedDate = opt.UpdatedDate,
                DeletedDate = opt.DeletedDate,
                QuizQuestion = new()
            })));

        CreateMap<IPaginate<QuizQuestion>, GetListResponse<GetQuizQuestionsByIdsListItemDto>>();

        CreateMap<QuizQuestion, GetByLessonIdQuizQuestionListItemDto>()
            .ForMember(destinationMember: qqdto => qqdto.Score, memberOptions: opt => opt.MapFrom(qq => qq.QuestionScore.Score))
            .ForMember(destinationMember: qqdto => qqdto.MaxScore, memberOptions: opt => opt.MapFrom(qq => qq.QuestionScore.MaxScore))
            .ForMember(destinationMember: qqdto => qqdto.Benefits, memberOptions: opt => opt.MapFrom(qq => qq.Benefits.Select(opt => new Benefit
            {
                Id = opt.Id,
                BenefitCode = opt.BenefitCode,
                Description = opt.Description,
                CreatedDate = opt.CreatedDate,
                UpdatedDate = opt.UpdatedDate,
                DeletedDate = opt.DeletedDate,
                QuizQuestions = new List<QuizQuestion>()
            })))
            .ForMember(destinationMember: qqdto => qqdto.Options, memberOptions: opt => opt.MapFrom(qq => qq.Options.Select(opt => new QuestionOption
            {
                Id = opt.Id,
                OptionText = opt.OptionText,
                IsCorrect = opt.IsCorrect,
                CreatedDate = opt.CreatedDate,
                UpdatedDate = opt.UpdatedDate,
                DeletedDate = opt.DeletedDate,
                QuizQuestion = new()
            })));

        CreateMap<IPaginate<QuizQuestion>, GetListResponse<GetByLessonIdQuizQuestionListItemDto>>();
    }
}