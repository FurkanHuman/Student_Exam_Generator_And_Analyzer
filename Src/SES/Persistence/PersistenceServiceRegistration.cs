using Application.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Repositories;

namespace Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BaseDbContext>(opt => opt.UseInMemoryDatabase("BaseDb"), ServiceLifetime.Transient);

        services.AddDbContextFactory<PostgreSqlDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("PostgreSqlDbConnectionStrings"),
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(PostgreSqlDbContext).Assembly.FullName);
                    npgsqlOptions.CommandTimeout(60);
                })
                .UseSnakeCaseNamingConvention();
        }, ServiceLifetime.Scoped);

        services.AddScoped<PostgreSqlDbContext>(provider => provider.GetRequiredService<IDbContextFactory<PostgreSqlDbContext>>().CreateDbContext());

        services.AddDbContextFactory<PostgreSqlUserDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("PostgreSqlUserDbConnectionStrings"),
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(PostgreSqlUserDbContext).Assembly.FullName);
                    npgsqlOptions.CommandTimeout(60);
                })
                .UseSnakeCaseNamingConvention();
        }, ServiceLifetime.Scoped);

        services.AddScoped<PostgreSqlUserDbContext>(provider => provider.GetRequiredService<IDbContextFactory<PostgreSqlUserDbContext>>().CreateDbContext());

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
        services.AddScoped<IStudentExamAnswerRepository, StudentExamAnswerRepository>();
        return services;
    }
}
