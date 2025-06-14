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

    public static readonly IDictionary<byte, string> SelectTypeOptions = new Dictionary<byte, string>
    {
        {1, "Açık Uçlu" },
        // { 2, "Kapalı Uçlu" },
        { 3, "Çoktan Seçmeli" },
        { 4, "Doğru/Yanlış" },
        { 5, "Boşluk Doldurma" },
        { 6, "Eşleştirme" },
        // { 7, "Sıralama"  }
    };

    public static readonly int DefaultOpenEndedOptions = 1;
    public static readonly int DefaultMultipleChoiceOptions = 4;
    public static readonly int DefaultTrueFalseOptions = 2;
    public static readonly int DefaultFillInTheBlankOptions = 1;
    public static readonly int DefaultMachingOptions = 2;


}
