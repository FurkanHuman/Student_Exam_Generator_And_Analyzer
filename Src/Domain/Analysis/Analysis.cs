using Domain.Common;

namespace Domain.Analysis;

public sealed class Analysis : Entity<int>
{
    public string Name { get; private set; }
    public string AIResponse { get; private set; }
    public int SemesterId { get; private set; }
    public int PrincipalId { get; private set; }
    public int SchoolId { get; private set; }
    public int LessonId { get; private set; }
    public int ReferenceBenefitId { get; private set; }

    private Analysis()
    {
        Name = string.Empty;
        AIResponse = string.Empty;
    }

    public static Analysis Create(string name,
                                  int semesterId,
                                  int principalId,
                                  int schoolId,
                                  int lessonId,
                                  int referenceBenefitId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Analysis name cannot be empty");

        return new Analysis
        {
            Name = name.Trim(),
            AIResponse = string.Empty,
            SemesterId = semesterId,
            PrincipalId = principalId,
            SchoolId = schoolId,
            LessonId = lessonId,
            ReferenceBenefitId = referenceBenefitId
        };
    }

    public void SetAIResponse(string response)
    {
        if (string.IsNullOrWhiteSpace(response))
            throw new DomainException("AI response cannot be empty");
        AIResponse = response;
    }
}
