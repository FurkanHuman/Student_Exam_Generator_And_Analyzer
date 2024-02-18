using Entity.Entities.Mains;
using System.Text.Json;

namespace App.QuizPoolFeature;

public class QuizPoolHandleV1 : IQuizPool
{
    private IList<QuizQuestionOlder> QuizQuestions { get; set; }
    private int Id { get; set; }

    public IList<QuizQuestionOlder>? GetQuestionPoolForName(string quizName)
    {
        string path = $@"{Environment.CurrentDirectory}\{quizName}\{quizName}.json";
        Console.WriteLine(path);

        IList<QuizQuestionOlder>? loadedQuizQuestions = new List<QuizQuestionOlder>();

        try
        {
            string fs = File.ReadAllText(path);

            loadedQuizQuestions = JsonSerializer.Deserialize<IList<QuizQuestionOlder>?>(fs);
        }
        catch (Exception)
        {

        }

        return loadedQuizQuestions;
    }

    public void QuestionPoolAddQuiz(string quizName)
    {
        string pathD = $@"{Environment.CurrentDirectory}\{quizName}\";
        string pathF = pathD + $@"{quizName}.json";

        if (!Directory.Exists(pathD))
            Directory.CreateDirectory(pathD);
        if (!File.Exists(pathF))
        {
            Console.WriteLine("Dosya bulunamadı!!! Oluşturuluyor");
            using FileStream fs = File.Create(pathF);
            fs.Close();
        }

        QuizQuestions = GetQuestionPoolForName(quizName);

        try
        {
            Id = QuizQuestions.Max(x => x.Id);

        }
        catch (Exception)
        {
            Id = 0;
        }

        Console.WriteLine("Çıkmak için F2 tuşuna basın...");
        while (Console.ReadKey().Key != ConsoleKey.F2)
        {
            QuizQuestionOlder quizQuestion = new();

            Console.Write("Soruyu girin: ");
            string? quiz = Console.ReadLine();

            if (quiz != null)
            {
                Id++;
                quizQuestion.LuckyFactor = 50;
                quizQuestion.Question = quiz;
                quizQuestion.Id = Id;

                QuizQuestions.Add(quizQuestion);
            }

            Console.WriteLine("devam etmek için bir tuaşa basın");
        }

        string jsonStr = JsonSerializer.Serialize(QuizQuestions.ToList());
        File.WriteAllText(pathF, jsonStr);
    }

    private static void ImputLuckyFactor(QuizQuestionOlder quizQuestion)
    {
        Console.Write("şans faktorü (varsayılan 50) : ");
        string? lucky = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(lucky))
        {
            quizQuestion.LuckyFactor = 50;
        }
        else
        {
            try
            {
                quizQuestion.LuckyFactor = int.Parse(lucky);
            }
            catch (Exception)
            {
                quizQuestion.LuckyFactor = 50;
            }
        }
    }

    public void QuestionPoolUpdateQuiz(string quizName)
    {
        string pathD = $@"{Environment.CurrentDirectory}\{quizName}\";
        string pathF = pathD + $@"{quizName}.json";
        QuizQuestions = GetQuestionPoolForName(quizName);

        while (Console.ReadKey().Key != ConsoleKey.F2)
        {
            Console.Write("Id: ");
            int quizId = int.Parse(Console.ReadLine());

            QuizQuestionOlder quizQuestion = QuizQuestions.First(x => x.Id == quizId);

            Console.WriteLine(quizQuestion.Question);
            Console.Write("Yeni Girdi: ");

            string? quiz = Console.ReadLine();
            quizQuestion.LuckyFactor = 0;

            if (quiz != null)
            {
                quizQuestion.Question = quiz;
                QuizQuestions.Remove(quizQuestion);
                QuizQuestions.Add(quizQuestion);
            }


            Console.WriteLine("devam etmek için bir tuaşa basın");
        }

        string jsonStr = JsonSerializer.Serialize(QuizQuestions.ToList());
        File.WriteAllText(pathF, jsonStr);
    }
}
