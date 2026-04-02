using Domain.Entities;
using Domain.ObjectValues;

namespace DomainTests.Mocks;

public class VehicleMock
{
    public static Vehicle ValidVehicle(int mileage = 5000)
    {
        return new(Guid.NewGuid(), new("AAA1A23"), mileage, []);
    }

    private static Vehicle GenerateVehicle(int cont)
    {
        string platePart = cont.ToString().PadLeft(3,'0');
        var plate = new Plate($"ASD-123{platePart}");
        return new Vehicle(Guid.NewGuid(), plate, 3 * cont, []);
    }

    public static List<Vehicle> ListValidVehicle()
    {
        var listVehicle = new List<Vehicle>
        {
            VehicleMock.GenerateVehicle(1),
            VehicleMock.GenerateVehicle(2),
            VehicleMock.GenerateVehicle(3)
        };

        return listVehicle;
    }
}
