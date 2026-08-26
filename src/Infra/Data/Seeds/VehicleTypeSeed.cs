using Domain.Constants;
using Domain.Entities;
using Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Seeds;

public class VehicleTypeSeed
{
    public static async Task SeedAsync(ZeloFrotaDbContext context)
    {
        if (await context.VehicleTypes.Where(vt => vt.Name == SeedConstants.DefaultVehicleType).AnyAsync())
            return;

        var vehicleType = new VehicleType(SeedConstants.DefaultVehicleType);

        context.VehicleTypes.Add(vehicleType);

        await context.SaveChangesAsync();

        Console.WriteLine("- Salvo 1 registro [VehicleTypeSeed]");
    }
}
