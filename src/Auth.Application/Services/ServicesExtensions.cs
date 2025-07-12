using Auth.Application.Services.Cryptography;
using Auth.Application.Services.TokenJwt;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application.Services;
public static class ServicesExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IEncrypter, Encrypter>();
        services.AddScoped<ITokenService, TokenService>();
        return services;
    }
}
