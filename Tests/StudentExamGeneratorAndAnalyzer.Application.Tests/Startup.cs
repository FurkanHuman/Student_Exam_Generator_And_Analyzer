using Microsoft.Extensions.DependencyInjection;
using StudentExamGeneratorAndAnalyzer.Application.Tests.DependencyResolvers;

namespace StudentExamGeneratorAndAnalyzer.Application.Tests;

public sealed class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddUsersServices();
        services.AddAuthServices();
    }
}
