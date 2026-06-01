using Infra.Data.Contexts;
using Infra.Data.Interseptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Extensions;

public static class RegisterContext
{
    public static IServiceCollection AddContexts(this IServiceCollection services, string? sqlQueryString)
    {
        services.AddScoped<SlowQueryInterceptor>();
        services.AddDbContext<ZeloFrotaDbContext>((sp, options) =>
        {
            options.UseSqlServer(sqlQueryString);
            options.AddInterceptors(sp.GetRequiredService<SlowQueryInterceptor>());
        });
        return services;
    }
}
