using Domain.Enum;
using Domain.ObjectValues;

namespace Domain.Entities;

public class Destination() : Base()
{

    public string ZipCode { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public string Locality { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Uf { get; set; } = "SE";

    public static Destination CreateDestination(string zipCode, string? address, string? neighborhood,
                        string? locality, string city, UF uf)
    {
        var zipCodeValided= new ZipCode(zipCode);
        if (string.IsNullOrWhiteSpace(city)) {
            throw new ArgumentException("O campo Cidade não podem ser vazio");
        }

        return new()
        {
            ZipCode = zipCodeValided.Value,
            Address = address?? string.Empty,
            Neighborhood = neighborhood ?? string.Empty,
            Locality = locality ?? string.Empty,
            City = city,
            Uf = uf.ToString()
        };
    }
}
