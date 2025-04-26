namespace BlazorWebUI.Client.Pages.Styles.QQ;

public static class QQStatic
{
    public static IDictionary<byte, string> SelectTypeOptions { get; set; } = new Dictionary<byte, string>
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
