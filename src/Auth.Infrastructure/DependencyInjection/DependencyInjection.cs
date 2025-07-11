using Auth.Core.Interfaces;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure.DependencyInjection;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration config)
    {
        AddDbContext(services, config);
        AddRepositories(services);

        return services;
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("PostgresString")));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}