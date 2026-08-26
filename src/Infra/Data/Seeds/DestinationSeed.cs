using Domain.Constants;
using Domain.Entities;
using Domain.ObjectValues;
using Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Seeds;

public class DestinationSeed
{
    public static async Task SeedAsync(ZeloFrotaDbContext context)
    {
        if (await context.Destinations.Where(d => d.ZipCode == SeedConstants.DestinationZipCode1 || d.ZipCode == SeedConstants.DestinationZipCode2).AnyAsync())
            return;

        var destination1 = Destination.CreateDestination(new ZipCode(SeedConstants.DestinationZipCode1), null, null, "BairroSeed1", "CidadeSeed1", Domain.Enum.UF.BA);
        var destination2 = Destination.CreateDestination(new ZipCode(SeedConstants.DestinationZipCode2), null, null, "BairroSeed2", "CidadeSeed2", Domain.Enum.UF.SP);

        context.Destinations.Add(destination1);
        context.Destinations.Add(destination2);

        await context.SaveChangesAsync();

        Console.WriteLine("- Salvo 2 registros [DestinationSeed]");
    }
}
