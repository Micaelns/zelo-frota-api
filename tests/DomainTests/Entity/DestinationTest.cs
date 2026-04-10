using Domain.Entities;
using Domain.Enum;

namespace DomainTests.Entity;

public class DestinationTest
{
    [Fact]
    public void CreateDestination_WithValidParams_ReturnDestinationObject()
    {
        var uf = (UF)Enum.Parse(typeof(UF), "BA", true);
        var zipCode = "48360-000";
        var city = "Acajutiba"; 
        var destination = Destination.CreateDestination(zipCode, null, null,null, city, uf);
        
        Assert.NotNull(destination);
        Assert.Equal(zipCode.Replace("-", ""), destination.ZipCode);
        Assert.Equal(city, destination.City);
        Assert.Equal("BA", destination.Uf);
    }

    [Theory]
    [InlineData("483600000", "Acajutiba", "CEP inválido")]
    [InlineData("48360 000", "Acajutiba", "CEP inválido")]
    [InlineData("48360-A00", "Acajutiba", "CEP inválido")]
    [InlineData("48360 0A0", "Acajutiba", "CEP inválido")]
    [InlineData("48360-00A", "Acajutiba", "CEP inválido")]
    [InlineData("48360-000", "", "O campo Cidade não podem ser vazio")]
    [InlineData("48360-000", "  ", "O campo Cidade não podem ser vazio")]
    public void CreateDestination_WithInvalidParams_ReturnException(string zipCode, string city, string errorMessage)
    {
        var uf = (UF)Enum.Parse(typeof(UF), "BA", true);
        
        var excecao = Assert.Throws<ArgumentException>(() => Destination.CreateDestination(zipCode, null, null, null, city, uf));

        Assert.Equal(errorMessage, excecao.Message);
    }
}
