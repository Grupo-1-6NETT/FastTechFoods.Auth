using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application.Services;
public static class CryptographyExtensions
{
    public static IServiceCollection RegisterEncrypter(this IServiceCollection services)
    {
        services.AddScoped<IEncrypter, Encrypter>();
        return services;
    }
}
