using Domain.Constants;
using Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Seeds;

public class DestinationCleanSeed
{
    public static async Task SeedCleanAsync(ZeloFrotaDbContext context)
    {
        var deleted = await context.Destinations
            .Where(ds => ds.ZipCode == SeedConstants.DestinationZipCode1 || ds.ZipCode == SeedConstants.DestinationZipCode2)
            .ExecuteDeleteAsync();

        if (deleted == 0) return;

        await context.SaveChangesAsync();

        Console.WriteLine($"- Limpo {deleted} registros [DestinationCleanSeed]");
    }
}
