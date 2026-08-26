using Infra.Data.Contexts;

namespace Infra.Data.Seeds;

public class DevelopmentDatabaseCleanSeeder
{
    public static async Task SeedCleanAsync(ZeloFrotaDbContext context)
    {
        await TravelCleanSeed.SeedCleanAsync(context);
        await DestinationCleanSeed.SeedCleanAsync(context);
        await VehicleCleanSeed.SeedCleanAsync(context);
        await VehicleTypeCleanSeed.SeedCleanAsync(context);
    }
}
