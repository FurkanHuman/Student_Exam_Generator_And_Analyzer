using Application.Services.Repositories;
using Moq;

namespace StudentExamGeneratorAndAnalyzer.Application.Tests.Mocks.Repositories.Auth;

public static class MockOtpAuthRepository
{
    public static IOtpAuthenticatorRepository GetOtpAuthenticatorMock()
    {
        var mockRepo = new Mock<IOtpAuthenticatorRepository>();
        return mockRepo.Object;
    }
}
