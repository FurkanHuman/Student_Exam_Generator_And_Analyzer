using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using StudentExamGeneratorAndAnalyzer.Application.Tests.Mocks.FakeDatas;
using System.Linq.Expressions;

namespace StudentExamGeneratorAndAnalyzer.Application.Tests.Mocks.Repositories.Auth;

public class MockUserRepository
{
    private readonly UserFakeData _userFakeData;

    public MockUserRepository(UserFakeData userFakeData)
    {
        _userFakeData = userFakeData;
    }

    public IUserRepository GetUserMockRepository()
    {
        #region GetAsync Mock

        var mockRepo = new Mock<IUserRepository>();
        mockRepo
            .Setup(s =>
                s.GetAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(
                (
                    Expression<Func<User, bool>> predicate,
                    Func<IQueryable<User>, IIncludableQueryable<User, object>>? include,
                    bool withDeleted,
                    bool enableTracking,
                    CancellationToken cancellationToken
                ) =>
                {
                    User? user = null;
                    if (predicate != null)
                        user = _userFakeData.Data.Where(predicate.Compile()).FirstOrDefault();
                    return user;
                }
            );

        #endregion GetAsync Mock

        return mockRepo.Object;
    }
}
