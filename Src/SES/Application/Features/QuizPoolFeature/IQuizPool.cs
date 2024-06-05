namespace Application.Features.QuizPoolFeature;

public interface IQuizPool
{
    void QuestionPoolAddQuiz(string quizName);
    void QuestionPoolUpdateQuiz(string quizName);
}
