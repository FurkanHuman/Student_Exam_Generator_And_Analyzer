using Application.Features.QuizPoolFeature;
using Application.Features.SchoolFeature;
using Application.Features.SemesterFeature.CRUD;
using Application.Features.Students.CRUD.Create;
using Application.PdfPageProduct.AnalysisPageFeature;
using Application.PdfPageProduct.AnalysisPageFeature.V1;
using Application.PdfPageProduct.ExamPageFeature;
using Application.PdfPageProduct.ExamPageFeature.V1;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class AppicationServiceRegistration
{
    public static IServiceCollection AddAppicationServiceRegistration(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddKeyedTransient<IExamPage, ExamPageWrittenV1>(1);
        services.AddKeyedTransient<IAnalsysPage, AnalsysPageWrittenV1>(1);
      
        services.AddSingleton<IQuizPool, QuizPoolHandleV1>();
        services.AddTransient<ISchoolService, SchoolV1>();
        services.AddTransient<ICreateStudent, CreateStudentV1>();
        services.AddTransient<ISemesterService, SemesterManager>();

        return services;
    }
}