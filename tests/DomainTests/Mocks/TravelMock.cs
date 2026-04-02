using Domain.Entities;

namespace DomainTests.Mocks;

public class TravelMock
{
    public static Travel ValidTravel()
    {
        return new(Guid.NewGuid(), Guid.NewGuid());
    }
}
