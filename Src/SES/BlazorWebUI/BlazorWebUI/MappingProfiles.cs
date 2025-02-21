using Application.Features.QuizQuestions.Queries.GetByLessonId;
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
            .ForMember(dest => dest.SelectedBenefitIds, opt => opt.MapFrom(src => src.Benefits.Select(b => b.Id)))
            .ForMember(dest => dest.QuestionOptions, opt => opt.MapFrom(src => src.Options));

        CreateMap<QuestionOption, QuestionOptionDto>()
            .ForMember(dest => dest.OptionText, opt => opt.MapFrom(src => src.OptionText))
            .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect));

    }
}
