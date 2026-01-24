using Domain.Common;
using Domain.Exam.ValueObjects;

namespace Domain.Exam;

public sealed class ExamConfiguration : Entity<int>
{
    public ConfigurationData Configuration { get; private set; }

    private ExamConfiguration()
    {
        Configuration = ConfigurationData.Create("{}");
    }

    public static ExamConfiguration Create(string configurationJson)
    {
        return new ExamConfiguration
        {
            Configuration = ConfigurationData.Create(configurationJson)
        };
    }

    public void UpdateConfiguration(string configurationJson) => Configuration = ConfigurationData.Create(configurationJson);
}
