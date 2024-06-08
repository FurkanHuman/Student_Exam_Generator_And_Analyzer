using Application.Services.Repositories;
using Moq;

namespace StudentExamGeneratorAndAnalyzer.Application.Tests.Mocks.Repositories.Auth;

public static class MockEmailAuthenticatorRepository
{
    public static IEmailAuthenticatorRepository GetEmailAuthenticatorRepositoryMock()
    {
        var mockRepo = new Mock<IEmailAuthenticatorRepository>();
        return mockRepo.Object;
    }
}
