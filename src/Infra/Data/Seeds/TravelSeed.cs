using Domain.Constants;
using Domain.Entities;
using Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Seeds;

public class TravelSeed
{
    public static async Task SeedAsync(ZeloFrotaDbContext context)
    {

        if (await context.Travels.Where(v => v.Vehicle.VehicleType.Name == SeedConstants.DefaultVehicleType).AnyAsync())
            return;

        var vehicles = await context.Vehicles
                            .AsNoTracking()
                            .Where(v => v.VehicleType.Name == SeedConstants.DefaultVehicleType)
                            .ToListAsync();

        if (vehicles.Count() == 0)
            return;

        var destinationZipCode1 = await context.Destinations.FirstOrDefaultAsync(d => d.ZipCode == SeedConstants.DestinationZipCode1);
        var destinationZipCode2 = await context.Destinations.FirstOrDefaultAsync(d => d.ZipCode == SeedConstants.DestinationZipCode2);

        if (destinationZipCode1 == null || destinationZipCode2 == null)
            return;

        int qtdTravels = 0;
        foreach (var vehicle in vehicles)
        {
            Travel[] travels = new Travel[SeedConstants.QtdTravelsPerVehicle];
            for (int j = 0; j < SeedConstants.QtdTravelsPerVehicle; j++)
            {
                var travelTmp = new Travel(vehicle.Id, j % 2 == 0 ? destinationZipCode1.Id : destinationZipCode2.Id);
                travelTmp.Starts(vehicle.Mileage, DateTime.UtcNow);
                if (j % 3 == 0)
                {
                    travelTmp.Ends(vehicle.Mileage + 500,100+(j%30),null);
                }
                travels[j] = travelTmp;
            }
            qtdTravels += travels.Count();
            context.Travels.AddRange(travels);
            await context.SaveChangesAsync();
        }
        Console.WriteLine($"- Salvo {qtdTravels} registro(s) [TravelSeed]");
    }
}