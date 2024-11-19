using Application.Services.Analyses;
using Application.Services.AuthenticatorService;
using Application.Services.AuthService;
using Application.Services.Benefits;
using Application.Services.CookiesService;
using Application.Services.Exams;
using Application.Services.LearningAreas;
using Application.Services.Lessons;
using Application.Services.PdfReaderService;
using Application.Services.Personels;
using Application.Services.Principals;
using Application.Services.QuestionOptions;
using Application.Services.QuestionScores;
using Application.Services.QuizQuestions;
using Application.Services.ReferenceBenefits;
using Application.Services.Schools;
using Application.Services.Semesters;
using Application.Services.StudentAnswers;
using Application.Services.StudentClasses;
using Application.Services.Students;
using Application.Services.SubLearningAreas;
using Application.Services.Teachers;
using Application.Services.UsersService;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using NArchitecture.Core.Application.Pipelines.Validation;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Logging.Abstraction;
using NArchitecture.Core.CrossCuttingConcerns.Logging.Configurations;
using NArchitecture.Core.CrossCuttingConcerns.Logging.Serilog.File;
using NArchitecture.Core.ElasticSearch;
using NArchitecture.Core.ElasticSearch.Models;
using NArchitecture.Core.Localization.Resource.Yaml.DependencyInjection;
using NArchitecture.Core.Mailing;
using NArchitecture.Core.Mailing.MailKit;
using NArchitecture.Core.Security.DependencyInjection;
using NArchitecture.Core.Security.JWT;
using System.Reflection;

namespace Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        MailSettings mailSettings,
        FileLogConfiguration fileLogConfiguration,
        ElasticSearchConfig elasticSearchConfig,
        TokenOptions tokenOptions
    )
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            configuration.AddOpenBehavior(typeof(AuthorizationBehavior<,>)); // Note: MediatR Blazor auth Problem ! 25.06.2024 solved this case
            configuration.AddOpenBehavior(typeof(CachingBehavior<,>));
            configuration.AddOpenBehavior(typeof(CacheRemovingBehavior<,>));
            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
            configuration.AddOpenBehavior(typeof(RequestValidationBehavior<,>));
            configuration.AddOpenBehavior(typeof(TransactionScopeBehavior<,>));
        });

        services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMailService, MailKitMailService>(_ => new MailKitMailService(mailSettings));
        services.AddSingleton<ILogger, SerilogFileLogger>(_ => new SerilogFileLogger(fileLogConfiguration));
        services.AddSingleton<IElasticSearch, ElasticSearchManager>(_ => new ElasticSearchManager(elasticSearchConfig));

        services.AddScoped<IAuthService, AuthManager>();
        services.AddScoped<IAuthenticatorService, AuthenticatorManager>();
        services.AddScoped<IUserService, UserManager>();
        services.AddScoped<ICookieService,CookieManager>();

        services.AddYamlResourceLocalization();

        services.AddSecurityServices<Guid, int, Guid>(tokenOptions);

        services.AddScoped<IAnalysisService, AnalysisManager>();
        services.AddScoped<IBenefitService, BenefitManager>();
        services.AddScoped<IExamService, ExamManager>();
        services.AddScoped<ILearningAreaService, LearningAreaManager>();
        services.AddScoped<ILearningAreaService, LearningAreaManager>();
        services.AddScoped<IPrincipalService, PrincipalManager>();
        services.AddScoped<IQuestionOptionService, QuestionOptionManager>();
        services.AddScoped<IQuestionScoreService, QuestionScoreManager>();
        services.AddScoped<IReferenceBenefitService, ReferenceBenefitManager>();
        services.AddScoped<ISchoolService, SchoolManager>();
        services.AddScoped<ISemesterService, SemesterManager>();
        services.AddScoped<IStudentService, StudentManager>();
        services.AddScoped<IStudentAnswerService, StudentAnswerManager>();
        services.AddScoped<ISubLearningAreaService, SubLearningAreaManager>();
        services.AddScoped<ITeacherService, TeacherManager>();
        services.AddScoped<IStudentClassService, StudentClassManager>();
        services.AddScoped<IQuizQuestionService, QuizQuestionManager>();
        services.AddScoped<IPersonelService, PersonelManager>();
        services.AddScoped<ILessonService, LessonManager>();
        services.AddScoped<IPersonelService, PersonelManager>();
        services.AddScoped<IPdfReaderService,PdfReaderManager>();
        return services;
    }

    public static IServiceCollection AddSubClassesOfType(
        this IServiceCollection services,
        Assembly assembly,
        Type type,
        Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null
    )
    {
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
        foreach (Type? item in types)
            if (addWithLifeCycle == null)
                services.AddScoped(item);
            else
                addWithLifeCycle(services, type);
        return services;
    }
}
