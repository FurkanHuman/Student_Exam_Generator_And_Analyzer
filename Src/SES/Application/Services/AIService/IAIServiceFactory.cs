namespace Application.Services.AIService;

public interface IAIServiceFactory
{
    IAIService GetService(string provider);
    IAIBatchService GetBatchService(string provider);
}