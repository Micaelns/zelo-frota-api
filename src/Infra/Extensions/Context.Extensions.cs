using Infra.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infra.Extensions;

public static class RegisterContext
{
    public static IServiceCollection AddPersistence(this IServiceCollection Services)
    {
        Services.AddDbContext<ZeloFrotaDbContext>(options =>
                options.UseSqlite("Data Source=ZeloFrota.db")
        );
        return Services;
    }
}
