using Domain.ObjectValues;

namespace DomainTests.ObjectValues;

public class PlateTest
{
    [Theory]
    [InlineData("ASD1D00")]
    [InlineData("ASD1000")]
    [InlineData("ASD-1D00")]
    [InlineData("ASD-1900")]
    public void Plate_withValidPlate_ValueHasAnValue(string value)
    {
        var plate = new Plate(value);

        Assert.NotNull(plate);
        Assert.Equal(value.Replace("-", "").ToUpper(), plate.Value);
    }

    [Theory]
    [InlineData("123 1234")]
    [InlineData("123-1234")]
    [InlineData("ASD 1D00")]
    [InlineData("ASD-100")]
    public void Plate_withInvalidPlate_ReturnArgumentException(string value)
    {
        var excecao = Assert.Throws<ArgumentException>(() => new Plate(value));
        
        Assert.Equal("Formato de placa inválido", excecao.Message);
    }
}
