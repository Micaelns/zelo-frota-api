using Domain.Entities;
using Domain.Enum;

namespace DomainTests.Mocks;

public class DestinationMock
{
    public static Destination ValidDestination()
    {
        var uf= (UF)Enum.Parse(typeof(UF), "BA", true);
        return  Destination.CreateDestination("48360-000",null, null, null,"Acajutiba", uf);
    }
}
