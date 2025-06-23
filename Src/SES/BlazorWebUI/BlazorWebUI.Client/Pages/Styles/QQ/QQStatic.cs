namespace BlazorWebUI.Client.Pages.Styles.QQ;

public static class QQStatic
{

    public static readonly IDictionary<byte, string> EvaluationStatusTUR = new Dictionary<byte, string>()
    {
        { 1, "Boş" },
        { 7, "Yanlış" },
        { 8, "Kısmen Doğru" },
        { 9, "Doğru" },
        { 0, "Henüz değerlendirilmedi" },
        { 3, "Alakasız cevap" },
        { 5, "Şüpheli intihal"},
        { 6, "Onaylı intihal" }
    };

    public static readonly IDictionary<byte, string> SelectTypeOptionsTUR = new Dictionary<byte, string>
    {
        {1, "Açık Uçlu" },
        // { 2, "Kapalı Uçlu" },
        { 3, "Çoktan Seçmeli" },
        { 4, "Doğru/Yanlış" },
        { 5, "Boşluk Doldurma" },
        { 6, "Eşleştirme" },
        // { 7, "Sıralama"  }
    };

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

    public static readonly char[] Alphabet = [.. Enumerable.Range('A', 26).Select(c => (char)c)];
    public static readonly int DefaultOpenEndedOptions = 1;
    public static readonly int DefaultMultipleChoiceOptions = 4;
    public static readonly int DefaultTrueFalseOptions = 2;
    public static readonly int DefaultFillInTheBlankOptions = 1;
    public static readonly int DefaultMachingOptions = 2;


}
