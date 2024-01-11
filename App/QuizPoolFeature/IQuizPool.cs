using Entity.Entities.Mains;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.QuizPoolFeature;

public interface IQuizPool
{
    void QuestionPoolAddQuiz(string quizName);

    IList<QuizQuestion>? GetQuestionPoolForName(string quizName);

    void QuestionPoolUpdateQuiz(string quizName);
}
