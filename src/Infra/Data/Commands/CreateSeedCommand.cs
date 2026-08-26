using Infra.Data.Contexts;
using Infra.Data.Seeds;

namespace Infra.Data.Commands;

public class CreateSeedCommand
{
    public static async Task ExecuteAsync(ZeloFrotaDbContext context)
    {
        await DevelopmentDatabaseSeeder.SeedAsync(context);
    }
}