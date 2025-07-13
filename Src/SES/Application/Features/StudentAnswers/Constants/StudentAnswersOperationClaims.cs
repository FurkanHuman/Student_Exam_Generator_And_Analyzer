namespace Application.Features.StudentAnswers.Constants;

public static class StudentAnswersOperationClaims
{
    private const string _section = "StudentAnswers";

    public const string Admin = $"{_section}.Admin";

    public const string Read = $"{_section}.Read";
    public const string Write = $"{_section}.Write";

    public const string Create = $"{_section}.Create";
    public const string Update = $"{_section}.Update";
    public const string Delete = $"{_section}.Delete";
    
    public const string CreateMultiple = $"{_section}.CreateMultiple";
    
    public const string CreateMultipleStudentAnswer = $"{_section}.CreateMultipleStudentAnswer";
}
