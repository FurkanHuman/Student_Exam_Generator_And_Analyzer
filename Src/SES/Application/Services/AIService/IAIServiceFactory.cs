namespace Application.Services.AIService;

public interface IAIServiceFactory
{
    IAIService GetService(string provider);
}