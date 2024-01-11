using App.AddSchool;
using App.AddStudent;
using App.PdfPageProduct.AnalysisPageFeature;
using App.PdfPageProduct.ExamPageFeature;
using App.QuizPoolFeature;
using App.SchoolFeature;
using Microsoft.Extensions.DependencyInjection;

namespace App;

public static class AppRegistration
{
    public static IServiceCollection AddAppServiceRegistration(this IServiceCollection services)
    {
        services.AddSingleton<IExamPage, PageWrittenExamV1>();
        services.AddSingleton<ISchool, SchoolV1>();
        services.AddSingleton<IAddStudent,AddStudentV1>();
        services.AddSingleton<IQuizPool, QuizPoolHandleV1>();
        services.AddSingleton<IAnalsysPage,AnalsysPageWrittenV1>();
        return services;
    }
}