namespace Infrastructure.Adapters.AIService;

public class BatchServiceConfiguration
{
    public bool Enabled { get; set; } = true;
    public int MaxBatchSize { get; set; } = 50;
    public int StatusCheckIntervalSeconds { get; set; } = 60;
    public int MaxWaitTimeHours { get; set; } = 24;
    public int ConcurrentRequestLimit { get; set; } = 10;
    public int RetryAttempts { get; set; } = 3;
    public int RetryDelaySeconds { get; set; } = 30;
    public bool RecordResults { get; set; } = true;
    public int TimeoutMinutes { get; set; } = 1440; // 24 hours
}