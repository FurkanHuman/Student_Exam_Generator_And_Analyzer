using App.ExamPage;
using Microsoft.Extensions.DependencyInjection;

namespace App;

public static class AppRegistration
{
    public static IServiceCollection AddAppServiceRegistration(this IServiceCollection services)
    {
        services.AddSingleton<IPage, PageWrittenExamV1>();
        return services;
    }
}