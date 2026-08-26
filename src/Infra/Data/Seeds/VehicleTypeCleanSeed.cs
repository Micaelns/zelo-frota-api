using Domain.Constants;
using Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Seeds;

public class VehicleTypeCleanSeed
{
    public static async Task SeedCleanAsync(ZeloFrotaDbContext context)
    {
        var deleted = await context.VehicleTypes
            .Where(vt => vt.Name == SeedConstants.DefaultVehicleType)
            .ExecuteDeleteAsync();

        if (deleted == 0) return;

        await context.SaveChangesAsync();

        Console.WriteLine("- Limpo 1 registro [VehicleTypeCleanSeed]");
    }
}
