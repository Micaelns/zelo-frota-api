using Infra.Data.Contexts;

namespace Infra.Data.Seeds;

public class DevelopmentDatabaseSeeder
{
    public static async Task SeedAsync(ZeloFrotaDbContext context)
    {
        await VehicleTypeSeed.SeedAsync(context);
        await VehicleSeed.SeedAsync(context);
        await DestinationSeed.SeedAsync(context);
        await TravelSeed.SeedAsync(context);
    }
}
