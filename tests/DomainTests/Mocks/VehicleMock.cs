using Domain.Entities;

namespace DomainTests.Mocks;

public class VehicleMock
{
    public static Vehicle ValidVehicle(int mileage = 5000)
    {
        return new(1, VehicleTypeMock.Carreta(), new("AAA1A23"), mileage, []);
    }
}
