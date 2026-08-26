using Domain.Constants;
using Domain.Entities;
using Domain.ObjectValues;
using Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;
namespace Infra.Data.Seeds;

public class VehicleSeed
{
    public static async Task SeedAsync(ZeloFrotaDbContext context)
    {
        var vehicleType = await context.VehicleTypes.FirstOrDefaultAsync(vt => vt.Name == SeedConstants.DefaultVehicleType);
        if (vehicleType is null)
            return;

        if (await context.Vehicles.Where(v => v.VehicleTypeId == vehicleType.Id).AnyAsync())
            return;

        var randonInt = new Random();
        var randonPlate = new Random();
        int mileage = 0;
        string plate = "";
        Vehicle[] vehicles = new Vehicle[SeedConstants.QtdItensVehicle];
        for ( int i=0; i<SeedConstants.QtdItensVehicle; i++)
        {
            mileage = randonInt.Next(SeedConstants.MinMileage, SeedConstants.MaxMileage);
            plate = (randonPlate.Next(0, 999).ToString()).PadLeft(3,'0');
            plate = $"AAA{plate.Substring(0, 1)}B{plate.Substring(1)}";
            vehicles[i] = new Vehicle(vehicleType.Id, new Plate(plate), mileage);
        }

        context.Vehicles.AddRange(vehicles);

        await context.SaveChangesAsync();
        Console.WriteLine($"- Salvo {vehicles.Length} registro(s) [VehicleSeed]");
    }
}
