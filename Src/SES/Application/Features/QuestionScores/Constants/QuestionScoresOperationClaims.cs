namespace Application.Features.QuestionScores.Constants;

public static class QuestionScoresOperationClaims
{
    private const string _section = "QuestionScores";

    public const string Admin = $"{_section}.Admin";

    public const string Read = $"{_section}.Read";
    public const string Write = $"{_section}.Write";

    public const string Create = $"{_section}.Create";
    public const string Update = $"{_section}.Update";
    public const string Delete = $"{_section}.Delete";
}