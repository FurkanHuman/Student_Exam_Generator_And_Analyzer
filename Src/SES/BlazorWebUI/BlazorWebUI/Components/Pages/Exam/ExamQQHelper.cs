using Application.Services.PdfFactory.CreateExamPdf.DTOs;
using BlazorWebUI.Client.Pages.Styles.QQ;
using SimpleBase;

namespace BlazorWebUI.Components.Pages.Exam
{
    /// <summary>
    /// This is a copy and adaptation of the QuizQuestionHelpers class within the "Application.Services.PdfFactory.CreateExamPdf.Helpers" namespace.
    /// It may be refactored and made suitable for use everywhere.
    /// maybe litle bit more changed.
    /// </summary>
    internal static class ExamQQHelper
    {
        // Orders quiz questions based on the provided order in ExamInfo.QQOrder
        internal static void OrderQuizQuestions(IList<QQBodyDto> qqBodies, ExamInfo examInfo, int seed)
        {
            if (examInfo.QQOrder == null || examInfo.QQOrder.Count == 0)
                return;

            Random random = new(seed);

            // Sort the QQOrder dictionary by key (order index)
            IList<KeyValuePair<int, int>> orderedPairs = examInfo.QQOrder
                                       .OrderBy(kvp => kvp.Key)
                                       .ToList();

            // Extract the IDs of the ordered questions
            var orderedIds = orderedPairs
                .Select(x => x.Value)
                .ToHashSet();

            // Create a list of ordered questions based on QQOrder
            IList<QQBodyDto?> ordered = orderedPairs
                .Select(p => qqBodies.FirstOrDefault(q => q.Id == p.Value))
                .Where(q => q != null)
                .ToList();

            // Identify and shuffle the unordered questions
            IList<QQBodyDto> unordered = qqBodies
                .Where(q => !orderedIds.Contains(q.Id))
                .OrderBy(q => random.Next())
                .ToList();

            // Insert unordered questions into random positions in the ordered list
            foreach (QQBodyDto q in unordered)
            {
                int insertIndex = random.Next(0, ordered.Count + 1);
                ordered.Insert(insertIndex, q);
            }

            // Update the original quizQuestions list with the new order

            ICollection<QQBodyDto> orderedQuestions = [.. ordered.Cast<QQBodyDto>()];

            // Clear the original list and add the ordered questions            
            qqBodies.Clear();
            foreach (QQBodyDto question in orderedQuestions)
                qqBodies.Add(question);
        }

        internal static void ShuffleAllQuestionOptions(ref IList<QQBodyDto> qqBodies, ExamInfo examInfo, int globalSeed)
        {
            // Check if randomization of options is enabled in the exam settings
            if (!examInfo.IsRandomizeOptions)
                return;

            // Iterate through each question and shuffle its options
            for (int i = 0; i < qqBodies.Count; i++)
            {
                QQBodyDto question = qqBodies[i];
                int localSeed = globalSeed * i; // Generate a unique seed for each question
                Random rng = new(localSeed);

                // Shuffle the options of the current question
                question.QuestionOptions = [.. question.QuestionOptions.OrderBy(_ => rng.Next())];
            }
        }

        // Shuffles the quiz questions if randomization is enabled in ExamInfo
        internal static void ShuffleQuizQuestions(ref IList<QQBodyDto> qqBodies, ExamInfo examInfo, int seed)
        {
            if (!examInfo.IsRandomizeQuestions)
                return;

            // Sort questions by ID before shuffling
            qqBodies = [.. qqBodies.OrderBy(q => q.Id)];

            // Shuffle questions using a random seed
            Random rnd = new(seed * qqBodies.Count);
            qqBodies = [.. qqBodies.OrderBy(q => rnd.Next(int.MinValue, int.MaxValue))];
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
    }
}
