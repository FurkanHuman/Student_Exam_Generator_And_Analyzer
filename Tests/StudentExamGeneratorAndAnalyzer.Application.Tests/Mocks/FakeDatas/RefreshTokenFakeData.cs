using Domain.Entities;
using NArchitecture.Core.Test.Application.FakeData;

namespace StudentExamGeneratorAndAnalyzer.Application.Tests.Mocks.FakeDatas;

public class RefreshTokenFakeData : BaseFakeData<RefreshToken, Guid>
{
    public override List<RefreshToken> CreateFakeData()
    {
        return [new() { UserId = UserFakeData.Ids[0], Token = "abc" }];
    }
}
