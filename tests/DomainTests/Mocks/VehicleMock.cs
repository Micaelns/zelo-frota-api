using Domain.Entities;
using Domain.ObjectValues;

namespace DomainTests.Mocks;

public class VehicleMock
{
    public static Vehicle ValidVehicle(int mileage = 5000)
    {
        return new(VehicleTypeMock.Carreta(), new("AAA1A23"), mileage, []);
    }

    private static Vehicle GenerateVehicle(int cont,VehicleType vehicleType)
    {
        string platePart = cont.ToString().PadLeft(3,'0');
        var plate = new Plate($"ASD-123{platePart}");
        return new Vehicle(vehicleType, plate, 3 * cont, []);
    }

    public static List<Vehicle> ListValidVehicle()
    {
        var listVehicle = new List<Vehicle>
        {
            VehicleMock.GenerateVehicle(1, VehicleTypeMock.Carreta()),
            VehicleMock.GenerateVehicle(2, VehicleTypeMock.Carreta()),
            VehicleMock.GenerateVehicle(3, VehicleTypeMock.Carreta())
        };

        return listVehicle;
    }
}
