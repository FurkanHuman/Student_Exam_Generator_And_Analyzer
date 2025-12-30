using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class ExamConfiguration : Entity<int>
{
    private string _configurationJsonStr;

    public string ConfigurationJsonStr
    {
        get => _configurationJsonStr;
        set
        {
            _configurationJsonStr = value;
            ConfigurationHash = ComputeHash(_configurationJsonStr);
        }
    }

    public string ConfigurationHash { get; private set; }

    public IList<Exam> Exams { get; set; }

    private static string ComputeHash(string input)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = System.Security.Cryptography.SHA256.HashData(bytes);
        return Convert.ToBase64String(hashBytes);
    }
}
