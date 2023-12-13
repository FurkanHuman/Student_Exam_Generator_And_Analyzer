using Microsoft.Extensions.DependencyInjection;

namespace App;

public static class AppRegistration
{
    public static IServiceCollection AddAppServiceRegistration(this IServiceCollection services)
    {
        //services.AddSingleton<IExamCodeGenerator, ExamCodeManager>(); todo: buraya geri dön
        return services;
    }
}