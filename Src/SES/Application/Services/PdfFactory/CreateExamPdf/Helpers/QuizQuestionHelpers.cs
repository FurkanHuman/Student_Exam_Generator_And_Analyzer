using Application.Services.PdfFactory.CreateExamPdf.DTOs;
using Domain.Entities;
using SimpleBase;

namespace Application.Services.PdfFactory.CreateExamPdf.Helpers;

internal static class QuizQuestionHelpers
{
    // Orders quiz questions based on the provided order in ExamInfo.QQOrder
    internal static void OrderQuizQuestions(IList<QuizQuestion> quizQuestions, ref ExamInfo examInfo, int seed)
    {
        if (examInfo.QQOrder == null || examInfo.QQOrder.Count == 0)
            return;

        Random random = new(seed);

        // Sort the QQOrder dictionary by key (order index)
        IList<KeyValuePair<int, int>> orderedPairs = [.. examInfo.QQOrder.OrderBy(kvp => kvp.Key)];

        // Extract the IDs of the ordered questions
        HashSet<int> orderedIds = [.. orderedPairs.Select(x => x.Value)];

        // Create a list of ordered questions based on QQOrder
        IList<QuizQuestion?> ordered = [.. orderedPairs
            .Select(p => quizQuestions.FirstOrDefault(q => q.Id == p.Value))
            .Where(q => q != null)];

        // Identify and shuffle the unordered questions
        IList<QuizQuestion> unordered = [.. quizQuestions
            .Where(q => !orderedIds.Contains(q.Id))
            .OrderBy(q => random.Next())];

        // Insert unordered questions into random positions in the ordered list
        foreach (QuizQuestion q in unordered)
        {
            int insertIndex = random.Next(0, ordered.Count + 1);
            ordered.Insert(insertIndex, q);
        }

        // Update the original quizQuestions list with the new order  
        quizQuestions = ordered.Cast<QuizQuestion>().ToList();
    }

    // Shuffles the quiz questions if randomization is enabled in ExamInfo
    internal static void ShuffleQuizQuestions(ref IList<QuizQuestion> quizQuestions, ref ExamInfo examInfo, int seed)
    {
        if (!examInfo.IsRandomizeQuestions)
            return;

        // Sort questions by ID before shuffling
        quizQuestions = [.. quizQuestions.OrderBy(q => q.Id)];

        // Shuffle questions using a random seed
        Random rnd = new(seed * quizQuestions.Count);
        quizQuestions = [.. quizQuestions.OrderBy(q => rnd.Next(int.MinValue, int.MaxValue))];
    }

    // Shuffles the options of a question if randomization is enabled in ExamInfo
    internal static void ShuffleQuestionOptions(ref IList<QuestionOption> options, ref ExamInfo examInfo, int seed)
    {
        // Sort options by ID before shuffling to ensure a consistent base order
        options = [.. options.OrderBy(o => o.Id)];

        // Shuffle options if randomization is enabled in the exam settings
        if (examInfo.IsRandomizeOptions)
        {
            Random rnd = new(seed);
            options = [.. options.OrderBy(_ => rnd.Next())];
        }
    }

    // Shuffles the options of all questions in the quiz if randomization is enabled in ExamInfo
    internal static void ShuffleAllQuestionOptions(ref IList<QuizQuestion> quizQuestions, ExamInfo examInfo, int globalSeed)
    {
        // Check if randomization of options is enabled in the exam settings
        if (!examInfo.IsRandomizeOptions)
            return;

        // Iterate through each question and shuffle its options
        for (int i = 0; i < quizQuestions.Count; i++)
        {
            QuizQuestion question = quizQuestions[i];
            int localSeed = globalSeed * i; // Generate a unique seed for each question
            Random rng = new(localSeed);

            // Shuffle the options of the current question
            question.Options = [.. question.Options.OrderBy(_ => rng.Next())];
        }
    }

    // Shuffles a string array using the Fisher-Yates algorithm
    internal static void ShuffleStringArray(ref string[] array, int seed)
    {
        Random rng = new(seed);
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }

    // Generates a Base32 string with a random number
    internal static string GenerateBase32String()
    {
        byte[] bytes = GenerateRandomSeedBytes32();

        // Encode the bytes to Base32
        return Base32.Crockford.Encode(bytes);
    }

    // Converts a Base32 string to a uint
    internal static uint DecodeBase32String(string base32String)
    {
        // Decode the Base32 string to bytes
        byte[] bytes = Base32.Crockford.Decode(base32String);
        // Reverse bytes if the system is little-endian
        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
        // Convert the bytes to a uint
        return BitConverter.ToUInt32(bytes);
    }

    // Converts a Base32 string to a human-readable format
    internal static string InsertDashInString(string str)
    {
        // Check if the string is not null or empty
        if (!string.IsNullOrEmpty(str))
            // Insert a dash after the 4th character
            return str.Insert(4, "-");
        return str;
    }

    // Converts a Base32 string format
    internal static void RemoveDashInString(ref string str)
    {
        // Check if the string is not null or empty
        if (!string.IsNullOrEmpty(str))
            // Remove the dash from the string
            str = str.Replace("-", "");
    }

    // Converts a byte array to a uint (uint 32-bit integer. for now, the system default)
    internal static uint ConvertSeedToUInt32(byte[] byteArray)
    {
        // Reverse bytes if the system is little-endian
        if (BitConverter.IsLittleEndian)
            Array.Reverse(byteArray);
        // Convert the bytes to a uint
        return BitConverter.ToUInt32(byteArray);
    }

    // Convert a byte array to a string CrockFord
    internal static string EncodeSeedToBase32(byte[] byteArray)
    {
        // Reverse bytes if the system is little-endian
        if (BitConverter.IsLittleEndian)
            Array.Reverse(byteArray);
        // Encode the bytes to Base32
        return Base32.Crockford.Encode(byteArray);
    }

    // Generates a random byte array representing a uint (32-bit unsigned integer)
    internal static byte[] GenerateRandomSeedBytes32()
    {
        // Generate a random number between int.MinValue and int.MaxValue
        uint randomNumber = (uint)new Random().Next(int.MinValue, int.MaxValue);

        // Convert the random number to bytes
        byte[] bytes = BitConverter.GetBytes(randomNumber);

        // Reverse bytes if the system is little-endian
        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
        return bytes;
    }
}
