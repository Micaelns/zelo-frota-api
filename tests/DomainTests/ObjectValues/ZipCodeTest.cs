using Domain.ObjectValues;

namespace DomainTests.ObjectValues;

public class ZipCodeTest
{
    [Theory]
    [InlineData("12345000")]
    [InlineData("12345-000")]
    public void ZipCode_withValidZipCode_ValueHasAnValue(string value)
    {
        var zipCode = new ZipCode(value);

        Assert.NotNull(zipCode);
        Assert.Equal(value.Replace("-", ""), zipCode.Value);
    }

    [Theory]
    [InlineData("12")]
    [InlineData("12345")]
    [InlineData("123456")]
    [InlineData("12345 678")]
    [InlineData("a2345-678")]
    [InlineData("1345-a78")]
    [InlineData("1234-5678")]
    [InlineData("123456-78")]
    public void ZipCode_withInvalidZipCodse_ReturnArgumentException(string value)
    {
        var excecao = Assert.Throws<ArgumentException>(() => new ZipCode(value));

        Assert.Equal("CEP inválido", excecao.Message);
    }
}
