using Application.Features.QuizQuestions.Queries.GetByLessonId;
using Application.Features.QuizQuestions.Queries.GetQuizQuestionsByIds;
using Application.Features.StudentAnswers.Commands.CreateMultiple;
using Application.Features.StudentExamAnswers.Commands.Create;
using AutoMapper;
using BlazorWebUI.Client.Pages.Styles.QQ;
using Domain.Entities;

namespace BlazorWebUI;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<GetByLessonIdQuizQuestionListItemDto, QQBodyDto>()
            .ForMember(dest => dest.SelectedQType, opt => opt.MapFrom(src => src.QuestionType))
            .ForMember(dest => dest.SelectedBenefits, opt => opt.MapFrom(dest => dest.Benefits.ToDictionary(d => d.Id, d => $"{d.BenefitCode} {d.Description}")))
            .ForMember(dest => dest.QuestionOptions, opt => opt.MapFrom(src => src.Options));

        CreateMap<QuestionOption, QuestionOptionDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.OptionText, opt => opt.MapFrom(src => src.OptionText))
            .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect));

        CreateMap<GetQuizQuestionsByIdsListItemDto, QQBodyDto>()
            .ForMember(dest => dest.SelectedQType, opt => opt.MapFrom(src => src.QuestionType))
            .ForMember(dest => dest.QuestionOptions, opt => opt.MapFrom(src => src.Options));
        CreateMap<QuestionAnswerDto, MultipleStudentAnswer>();

        CreateMap<QuestionAnswerDto, StudentQuestionAnswerDto>();

    }
}
