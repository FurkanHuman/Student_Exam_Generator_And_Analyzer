using App.AddSchool;
using App.AddStudent;
using App.PdfPageProduct.AnalysisPageFeature;
using App.PdfPageProduct.AnalysisPageFeature.V1;
using App.PdfPageProduct.ExamPageFeature;
using App.PdfPageProduct.ExamPageFeature.V1;
using App.QuizPoolFeature;
using App.SchoolFeature;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace App;

public static class AppRegistration
{
    public static IServiceCollection AddAppServiceRegistration(this IServiceCollection services)
    {

        services.AddKeyedTransient<IExamPage, ExamPageWrittenV1>(1);
        services.AddKeyedTransient<IAnalsysPage, AnalsysPageWrittenV1>(1);
        services.AddSingleton<ISchool, SchoolV1>();
        services.AddSingleton<IAddStudent, AddStudentV1>();
        services.AddSingleton<IQuizPool, QuizPoolHandleV1>();
        return services;
    }
}