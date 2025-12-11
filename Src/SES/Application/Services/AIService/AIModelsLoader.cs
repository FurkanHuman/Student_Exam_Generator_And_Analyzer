namespace Application.Services.AIService;

public static class AIModelsLoader
{
    private static readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "Services", "AIService", "Resources", "AIModels.csv");
    private static Dictionary<string, string>? _aiProviders;
    private static Dictionary<string, Dictionary<string, string>>? _providerModels;

    public static Dictionary<string, string> GetAIProviders()
    {
        if (_aiProviders == null)
            LoadModelsFromCsv();

        return _aiProviders!;
    }

    public static Dictionary<string, Dictionary<string, string>> GetProviderModels()
    {
        if (_providerModels == null)
            LoadModelsFromCsv();

        return _providerModels!;
    }

    private static void LoadModelsFromCsv()
    {
        _aiProviders = [];
        _providerModels = [];

        string[] lines = File.ReadAllLines(_filePath);
        if (!File.Exists(_filePath))
            throw new FileNotFoundException($"CSV file not found: {_filePath}");

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] parts = line.Split(',');
            if (parts.Length < 4)
                continue;

            string model = parts[0].Trim();
            string shortName = parts[1].Trim();
            string provider = parts[2].Trim();
            string providerDisplayName = parts[3].Trim();

            if (!_aiProviders.ContainsKey(provider))
                _aiProviders[provider] = providerDisplayName;

            if (!_providerModels.ContainsKey(provider))
                _providerModels[provider] = [];

            _providerModels[provider][model] = shortName;
        }
    }
}