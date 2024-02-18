using Entity.Entities.Mains;

namespace App.QuizPoolFeature;

public interface IQuizPool
{
    void QuestionPoolAddQuiz(string quizName);

    IList<QuizQuestionOlder>? GetQuestionPoolForName(string quizName);

    void QuestionPoolUpdateQuiz(string quizName);
}
