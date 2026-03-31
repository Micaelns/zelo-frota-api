using Domain.Entities;

namespace DomainTests.Mocks;

public class VehicleTypeMock
{
    public static VehicleType Carreta()
    {
        return new(1, "Cavalinho");
    }
}
