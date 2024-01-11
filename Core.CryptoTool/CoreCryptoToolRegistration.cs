using Core.CryptoTool.FileOperations;
using Microsoft.Extensions.DependencyInjection;

namespace Core.CryptoTool;

public static class CoreCryptoToolRegistration
{
    public static IServiceCollection AddCoreCryptoToolServiceRegistration(this IServiceCollection services)
    {
        services.AddScoped<IEncrypttFile,EncryptFileOperationsV1>();

        return services;
    }
}