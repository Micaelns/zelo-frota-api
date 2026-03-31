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
    public void Ends_WithoutParams_SetEndNow()
    {
        var travel = TravelMock.ValidTravel();
        var endMileage = travel.Vehicle.Mileage + 4000;

        travel.Starts(null);
        travel.Ends(endMileage,null);

        Assert.Equal(endMileage, travel.FinishedMileage);
        Assert.NotNull(travel.End);
    }

    [Fact]
    public void Starts_WithValidParam_SetCorrectEnd()
    {
        var travel = TravelMock.ValidTravel();
        var endMileage = travel.Vehicle.Mileage + 4000;
        var now = DateTime.Now;
        var tomorrow = DateTime.Now.AddDays(1);

        travel.Starts(now);
        travel.Ends(endMileage, tomorrow);

        Assert.Equal(endMileage, travel.FinishedMileage);
        Assert.Equal(tomorrow, travel.End);
    }

    [Fact]
    public void Starts_WithInvalidFinishMileage_returnException()
    {
        var travel = TravelMock.ValidTravel();

        travel.Starts(null);
        var excecao = Assert.Throws<ArgumentException>(() => travel.Ends(-1, null));

        Assert.Equal("Quilometragem informada é inválida", excecao.Message);
    }

    [Fact]
    public void Starts_WithoutStarts_returnException()
    {
        var travel = TravelMock.ValidTravel();

        var excecao = Assert.Throws<ArgumentException>(() => travel.Ends(2, null));

        Assert.Equal("A viagem não foi iniciada", excecao.Message);
    }

    [Fact]
    public void Starts_WithEndsBeforeStarts_SetCorrectEnd()
    {
        var travel = TravelMock.ValidTravel();
        var endMileage = travel.Vehicle.Mileage - 500;
        var now = DateTime.Now;
        var tomorrow = DateTime.Now.AddDays(1);

        travel.Starts(tomorrow);
        var excecao = Assert.Throws<ArgumentException>(() => travel.Ends(endMileage, now));

        Assert.Equal("A viagem não pode finalizar antes de iniciar", excecao.Message);
    }
}
