using Infra.Data.Contexts;
using Infra.Data.Interseptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Extensions;

public static class RegisterContext
{
    public static IServiceCollection AddContexts(this IServiceCollection services)
    {
        services.AddScoped<SlowQueryInterceptor>();
        services.AddDbContext<ZeloFrotaDbContext>((sp, options) =>
        {
            options.UseSqlite("Data Source=ZeloFrota.db");
            options.AddInterceptors(sp.GetRequiredService<SlowQueryInterceptor>());
        }
        );
        return services;
    }
}
