using Domain.Entities;
using Domain.Enum;
using Domain.ObjectValues;

namespace DomainTests.Entity;

public class DestinationTest
{
    [Fact]
    public void CreateDestination_WithValidParams_ReturnDestinationObject()
    {
        var uf = (UF)Enum.Parse(typeof(UF), "BA", true);
        var zipCode = new ZipCode("48360-000");
        var city = "Acajutiba"; 
        var destination = Destination.CreateDestination(zipCode, null, null,null, city, uf);
        
        Assert.NotNull(destination);
        Assert.Equal(zipCode.Value, destination.ZipCode);
        Assert.Equal(city, destination.City);
        Assert.Equal("BA", destination.Uf);
    }

    [Theory]
    [InlineData("48360-000", "", "O campo Cidade não podem ser vazio")]
    [InlineData("48360-000", "  ", "O campo Cidade não podem ser vazio")]
    public void CreateDestination_WithInvalidParams_ReturnException(string zipCode, string city, string errorMessage)
    {
        var uf = (UF)Enum.Parse(typeof(UF), "BA", true);
        var zipCodeOBj = new ZipCode(zipCode);

        var excecao = Assert.Throws<ArgumentException>(() => Destination.CreateDestination(zipCodeOBj, null, null, null, city, uf));

        Assert.Equal(errorMessage, excecao.Message);
    }
}
