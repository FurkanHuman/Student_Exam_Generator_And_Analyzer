using Application.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NArchitecture.Core.Persistence.DependencyInjection;
using Persistence.Contexts;
using Persistence.Repositories;

namespace Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BaseDbContext>(opt => opt.UseInMemoryDatabase("BaseDb"));

        services.AddDbContext<PostgreSqlDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("PostgreSqlDbConnectionStrings"),
            m => m.MigrationsAssembly(typeof(PostgreSqlDbContext).Assembly.FullName)).UseSnakeCaseNamingConvention());
        services.BuildServiceProvider().GetRequiredService<PostgreSqlDbContext>().Database.Migrate();

        services.AddDbContext<PostgreSqlUserDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("PostgreSqlUserDbConnectionStrings"),
                m => m.MigrationsAssembly(typeof(PostgreSqlUserDbContext).Assembly.FullName)).UseSnakeCaseNamingConvention());
        services.BuildServiceProvider().GetRequiredService<PostgreSqlUserDbContext>().Database.Migrate();

        services.AddScoped<IAnalysisRepository, AnalysisRepository>();
        services.AddScoped<IBenefitRepository, BenefitRepository>();
        services.AddScoped<IExamRepository, ExamRepository>();
        services.AddScoped<ILearningAreaRepository, LearningAreaRepository>();
        services.AddScoped<ILearningAreaRepository, LearningAreaRepository>();
        services.AddScoped<IPrincipalRepository, PrincipalRepository>();
        services.AddScoped<IQuestionOptionRepository, QuestionOptionRepository>();
        services.AddScoped<IQuestionScoreRepository, QuestionScoreRepository>();
        services.AddScoped<IReferenceBenefitRepository, ReferenceBenefitRepository>();
        services.AddScoped<ISchoolRepository, SchoolRepository>();
        services.AddScoped<ISemesterRepository, SemesterRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IStudentAnswerRepository, StudentAnswerRepository>();
        services.AddScoped<ISubLearningAreaRepository, SubLearningAreaRepository>();
        services.AddScoped<ITeacherRepository, TeacherRepository>();
        services.AddScoped<IStudentClassRepository, StudentClassRepository>();
        services.AddScoped<IQuizQuestionRepository, QuizQuestionRepository>();
        services.AddScoped<IPersonelRepository, PersonelRepository>();
        services.AddScoped<ILessonRepository, LessonRepository>();
        return services;
    }
}
