using Domain.Constants;
using Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Seeds;

public class VehicleCleanSeed
{
    public static async Task SeedCleanAsync(ZeloFrotaDbContext context)
    {
        var deleted = await context.Vehicles
            .Where(v => v.VehicleType.Name == SeedConstants.DefaultVehicleType)
            .ExecuteDeleteAsync();

        if (deleted == 0) return;

        await context.SaveChangesAsync();

        Console.WriteLine($"- Limpo {deleted} registros [ VehicleCleanSeed]");
    }
}
