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
        services.AddDbContext<BaseDbContext>(options => options.UseInMemoryDatabase("BaseDb"));
        services.AddDbContext<PostgreSqlDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("PostgreSqlDbConnectionStrings")).UseSnakeCaseNamingConvention());
        // services.AddDbMigrationApplier(buildServices => buildServices.GetRequiredService<BaseDbContext>());
        services.AddDbMigrationApplier(buildServices => buildServices.GetRequiredService<PostgreSqlDbContext>());

        services.AddScoped<IEmailAuthenticatorRepository, EmailAuthenticatorRepository>();
        services.AddScoped<IOperationClaimRepository, OperationClaimRepository>();
        services.AddScoped<IOtpAuthenticatorRepository, OtpAuthenticatorRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserOperationClaimRepository, UserOperationClaimRepository>();

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
