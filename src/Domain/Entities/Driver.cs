using Domain.ObjectValues;

namespace Domain.Entities;

public class Driver : Base
{
    public string Name { get; set; } = string.Empty; 
    public string? Description { get; set; }
    public DriverLicense DriverLicense { get; set; }

    protected Driver() {
        DriverLicense = new(null, Enum.LicenseCategory.None, DateTime.UtcNow);
    }

    public Driver(string name, string? description, DriverLicense driverLicense)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("O campo Nome não podem ser vazio");
        }

        Name = name;
        Description = description;
        DriverLicense = driverLicense;
    }
}
