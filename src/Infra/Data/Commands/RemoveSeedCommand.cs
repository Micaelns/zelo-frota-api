using Infra.Data.Contexts;
using Infra.Data.Seeds;

namespace Infra.Data.Commands;

public class RemoveSeedCommand
{
    public static async Task ExecuteAsync(ZeloFrotaDbContext context)
    {
        await DevelopmentDatabaseCleanSeeder.SeedCleanAsync(context);
    }
}
