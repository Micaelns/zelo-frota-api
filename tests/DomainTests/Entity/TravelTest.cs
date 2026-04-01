using DomainTests.Mocks;

namespace DomainTests.Entity;

public class TravelTest
{
    [Fact]
    public void Starts_WithoutParams_SetStartNow()
    {
        var travel= TravelMock.ValidTravel();

        travel.Starts(null);

        Assert.Equal(travel.Vehicle.Mileage, travel.StartedMileage);
        Assert.NotNull(travel.Start);
    }

    [Fact]
    public void Starts_WithValidParam_SetCorrectStart()
    {
        var travel = TravelMock.ValidTravel();
        var tomorrow = DateTime.Now.AddDays(1);

        travel.Starts(tomorrow);

        Assert.Equal(travel.Vehicle.Mileage, travel.StartedMileage);
        Assert.Equal(tomorrow, travel.Start);
    }

    [Fact]
    public void Ends_WithValidParam_CalculateCorrectlyAutonomy()
    {
        var fuelQTD = 800;
        var travel = TravelMock.ValidTravel();
        var expectTraveled = 4000;
        var expectAutonomy = expectTraveled / fuelQTD;
        var endMileage = travel.Vehicle.Mileage + expectTraveled;
        var now = DateTime.Now;
        var tomorrow = DateTime.Now.AddDays(1);

        travel.Starts(now);
        travel.Ends(endMileage, fuelQTD, tomorrow);

        Assert.Equal(expectAutonomy, travel.Autonomy);
    }

    [Fact]
    public void Ends_WithoutParams_SetEndNow()
    {
        var fuelQTD = 800;
        var travel = TravelMock.ValidTravel();
        var endMileage = travel.Vehicle.Mileage + 4000;

        travel.Starts(null);
        travel.Ends(endMileage, fuelQTD, null);

        Assert.Equal(endMileage, travel.FinishedMileage);
        Assert.NotNull(travel.End);
    }

    [Fact]
    public void Ends_WithValidParam_SetCorrectEnd()
    {
        var fuelQTD = 800;
        var travel = TravelMock.ValidTravel();
        var endMileage = travel.Vehicle.Mileage + 4000;
        var now = DateTime.Now;
        var tomorrow = DateTime.Now.AddDays(1);

        travel.Starts(now);
        travel.Ends(endMileage, fuelQTD, tomorrow);

        Assert.Equal(endMileage, travel.FinishedMileage);
        Assert.Equal(tomorrow, travel.End);
    }

    [Fact]
    public void Ends_WithInvalidFinishMileage_returnException()
    {
        var fuelQTD = 800;
        var travel = TravelMock.ValidTravel();

        travel.Starts(null);
        var excecao = Assert.Throws<ArgumentException>(() => travel.Ends(-1, fuelQTD, null));

        Assert.Equal("Quilometragem informada é inválida", excecao.Message);
    }

    [Fact]
    public void Ends_WithoutStarts_returnException()
    {
        var fuelQTD = 800;
        var travel = TravelMock.ValidTravel();

        var excecao = Assert.Throws<ArgumentException>(() => travel.Ends(2, fuelQTD, null));

        Assert.Equal("A viagem não foi iniciada", excecao.Message);
    }

    [Fact]
    public void Ends_WithEndsBeforeStarts_SetCorrectEnd()
    {
        var fuelQTD = 800;
        var travel = TravelMock.ValidTravel();
        var endMileage = travel.Vehicle.Mileage - 500;
        var now = DateTime.Now;
        var tomorrow = DateTime.Now.AddDays(1);

        travel.Starts(tomorrow);
        var excecao = Assert.Throws<ArgumentException>(() => travel.Ends(endMileage, fuelQTD, now));

        Assert.Equal("A viagem não pode finalizar antes de iniciar", excecao.Message);
    }
}
