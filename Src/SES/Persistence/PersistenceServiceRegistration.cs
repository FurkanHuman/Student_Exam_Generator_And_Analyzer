using NArchitecture.Core.Persistence.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using App.Repositories;
using Persistence.Repositories;

namespace Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        _ = services.AddDbContext<BaseDbContext>(options => options.UseInMemoryDatabase("BaseDb"));
        _ = services.AddDbMigrationApplier(buildServices => buildServices.GetRequiredService<BaseDbContext>());

        _ = services.AddDbContext<PostgreSqlDbContext>(opt =>
            {
                opt.UseNpgsql("Host=192.168.1.11;Username=postgres;Password=12345;Database=SES_DB").UseSnakeCaseNamingConvention();
            });
        _ = services.AddDbMigrationApplier(buildServices => buildServices.GetRequiredService<PostgreSqlDbContext>());

        _ = services.AddScoped<IAnalysisHeaderRepository, AnalysisHeaderRepository>();
        _ = services.AddScoped<IBenefitRepository, BenefitRepository>();
        _ = services.AddScoped<IExamRepository, ExamRepository>();
        _ = services.AddScoped<ILearningAreaRepository, LearningAreaRepository>();
        _ = services.AddScoped<IPrincipalRepository, PrincipalRepository>();
        _ = services.AddScoped<IQuestionScoreRepository, QuestionScoreRepository>();
        _ = services.AddScoped<IQuizForAnswersRepository, QuizForAnswersRepository>();
        _ = services.AddScoped<IQuizQuestionRepository, QuizQuestionRepository>();
        _ = services.AddScoped<IReferenceBenefitRepository, ReferenceBenefitRepository>();
        _ = services.AddScoped<ISchoolRepository, SchoolRepository>();
        _ = services.AddScoped<IStudentQuizAnswerRepository, StudentQuizAnswerRepository>();
        _ = services.AddScoped<IStudentRepository, StudentRepository>();
        _ = services.AddScoped<ISubLearningAreaRepository, SubLearningAreaRepository>();
        _ = services.AddScoped<ITeacherRepository, TeacherRepository>();

        //_ = services.AddScoped<IEmailAuthenticatorRepository, EmailAuthenticatorRepository>();
        //_ = services.AddScoped<IOperationClaimRepository, OperationClaimRepository>();
        //_ = services.AddScoped<IOtpAuthenticatorRepository, OtpAuthenticatorRepository>();
        //_ = services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        //_ = services.AddScoped<IUserRepository, UserRepository>();
        //_ = services.AddScoped<IUserOperationClaimRepository, UserOperationClaimRepository>();

        return services;
    }
}
