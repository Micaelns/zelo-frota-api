using DomainTests.Mocks;

namespace DomainTests.Entity;

public class TravelTest
{
    [Fact]
    public void Starts_WithoutParams_SetStartNow()
    {
        var travel= TravelMock.ValidTravel();
        var currentMileage = 2000;

        travel.Starts(currentMileage, null);

        Assert.Equal(currentMileage, travel.StartedMileage);
        Assert.NotNull(travel.Start);
    }

    [Fact]
    public void Starts_WithValidParam_SetCorrectStart()
    {
        var travel = TravelMock.ValidTravel();
        var tomorrow = DateTime.Now.AddDays(1);
        var currentMileage = 2000;

        travel.Starts(currentMileage, tomorrow);

        Assert.Equal(currentMileage, travel.StartedMileage);
        Assert.Equal(tomorrow, travel.Start);
    }

    [Fact]
    public void Starts_WithValidParamTwice_ReturnException()
    {
        var travel = TravelMock.ValidTravel();
        var tomorrow = DateTime.Now.AddDays(1);
        var currentMileage = 2000;

        travel.Starts(currentMileage, tomorrow);

        var excecao = Assert.Throws<InvalidOperationException>(() => travel.Starts(currentMileage, tomorrow));

        Assert.Equal("Viagem já iniciada", excecao.Message);

    }

    [Fact]
    public void Ends_WithValidParam_CalculateCorrectlyAutonomy()
    {
        var fuelQTD = 800;
        var travel = TravelMock.ValidTravel();
        var expectTraveled = 4000;
        var expectAutonomy = expectTraveled / fuelQTD;
        var currentMileage = 2000;
        var endMileage = currentMileage + expectTraveled;
        var now = DateTime.Now;
        var tomorrow = DateTime.Now.AddDays(1);

        travel.Starts(currentMileage,now);
        travel.Ends(endMileage, fuelQTD, tomorrow);

        Assert.Equal(expectAutonomy, travel.Autonomy);
    }

    [Fact]
    public void Ends_WithoutParams_SetEndNow()
    {
        var fuelQTD = 800;
        var currentMileage = 2000;
        var travel = TravelMock.ValidTravel();
        var endMileage = currentMileage + 4000;

        travel.Starts(currentMileage, null);
        travel.Ends(endMileage, fuelQTD, null);

        Assert.Equal(endMileage, travel.FinishedMileage);
        Assert.NotNull(travel.End);
    }

    [Fact]
    public void Ends_WithValidParam_SetCorrectEnd()
    {
        var fuelQTD = 800;
        var travel = TravelMock.ValidTravel();
        var currentMileage = 2000;
        var endMileage = currentMileage + 4000;
        var now = DateTime.Now;
        var tomorrow = DateTime.Now.AddDays(1);

        travel.Starts(currentMileage, now);
        travel.Ends(endMileage, fuelQTD, tomorrow);

        Assert.Equal(endMileage, travel.FinishedMileage);
        Assert.Equal(tomorrow, travel.End);
    }

    [Fact]
    public void Ends_WithInvalidFinishMileage_returnException()
    {
        var fuelQTD = 800;
        var currentMileage = 2000;
        var travel = TravelMock.ValidTravel();

        travel.Starts(currentMileage,null);
        var excecao = Assert.Throws<ArgumentException>(() => travel.Ends(-1, fuelQTD, null));

        Assert.Equal("Quilometragem informada é inválida", excecao.Message);
    }

    [Fact]
    public void Ends_WithoutStarts_returnException()
    {
        var fuelQTD = 800;
        var travel = TravelMock.ValidTravel();

        var excecao = Assert.Throws<InvalidOperationException>(() => travel.Ends(2, fuelQTD, null));

        Assert.Equal("A viagem não foi iniciada", excecao.Message);
    }

    [Fact]
    public void Ends_WithEndsBeforeStarts_SetCorrectEnd()
    {
        var fuelQTD = 800;
        var currentMileage = 2000;
        var travel = TravelMock.ValidTravel();
        var endMileage = currentMileage - 500;
        var now = DateTime.Now;
        var tomorrow = DateTime.Now.AddDays(1);

        travel.Starts(currentMileage, tomorrow);
        var excecao = Assert.Throws<ArgumentException>(() => travel.Ends(endMileage, fuelQTD, now));

        Assert.Equal("A viagem não pode finalizar antes de iniciar", excecao.Message);
    }
}
