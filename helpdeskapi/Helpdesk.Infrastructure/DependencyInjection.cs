using Helpdesk.Domain.Repositories;
using Helpdesk.Infrastructure.Abstractions;
using Helpdesk.Infrastructure.Legacy;
using Helpdesk.Infrastructure.Legacy.Entities;
using Helpdesk.Infrastructure.Persistence;
using Helpdesk.Infrastructure.Repositories;
using Helpdesk.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Helpdesk.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("HelpdeskDatabase");

        services.AddDbContext<HelpdeskDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sql =>
            {
                sql.EnableRetryOnFailure();
            });
        });

        services.AddDbContext<LegacyDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sql =>
            {
                sql.EnableRetryOnFailure();
            });
        });

        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<TokenSpService>();
        services.AddScoped<LegacyDataAccess>();
        services.AddScoped<AuthService>();
        services.AddScoped<CategoryService>();
        services.AddScoped<ISupportUserService, SupportUserService>();

        return services;
    }
}
