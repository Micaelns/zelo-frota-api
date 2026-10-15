using Domain.Enum;
using Domain.ObjectValues;

namespace DomainTests.ObjectValues;

public class DriverLicenseTest
{
    [Fact]
    public void DriverLicense_withValidParams_NoReturnException()
    {
        var numberLicense = "123456789";
        var licenseCategory = LicenseCategory.AB;
        var expirationDate = DateTime.UtcNow.AddYears(5);
        var driverLicense = new DriverLicense(numberLicense, licenseCategory, expirationDate);

        Assert.NotNull(driverLicense);
        Assert.Equal(driverLicense.NumberLicense, numberLicense);
    }

    [Fact]
    public void IsExpired_withDriverLicenseNoExpired_ReturnFalse()
    {
        var numberLicense = "123456789";
        var licenseCategory = LicenseCategory.AB;
        var expirationDate = DateTime.UtcNow.AddYears(5);
        var driverLicense = new DriverLicense(numberLicense, licenseCategory, expirationDate);

        Assert.False(driverLicense.IsExpired());
    }

    [Fact]
    public void IsExpired_withDriverLicenseExpired_ReturnTrue()
    {
        var numberLicense = "123456789";
        var licenseCategory = LicenseCategory.AB;
        var expirationDate = DateTime.UtcNow.AddDays(-1);
        var driverLicense = new DriverLicense(numberLicense, licenseCategory, expirationDate);

        Assert.True(driverLicense.IsExpired());
    }

    [Theory]
    [InlineData(LicenseCategory.A)]
    [InlineData(LicenseCategory.AB)]
    [InlineData(LicenseCategory.AC)]
    [InlineData(LicenseCategory.AD)]
    [InlineData(LicenseCategory.AE)]
    public void CanDrive_withCanDriveMotoCycleWithDriverLicenseAX_ReturnTrue(LicenseCategory license)
    {
        var numberLicense = "123456789";
        var licenseCategory = license;
        var expirationDate = DateTime.UtcNow.AddDays(10);
        var driverLicense = new DriverLicense(numberLicense, licenseCategory, expirationDate);

        Assert.True(driverLicense.CanDrive(LicenseCategory.A));
    }

    [Theory]
    [InlineData(LicenseCategory.B)]
    [InlineData(LicenseCategory.C)]
    [InlineData(LicenseCategory.D)]
    [InlineData(LicenseCategory.E)]
    public void CanDrive_withCanDriveMotoCycleWithOutDriverLicenseA_ReturnFalse(LicenseCategory license)
    {
        var numberLicense = "123456789";
        var licenseCategory = LicenseCategory.A;
        var expirationDate = DateTime.UtcNow.AddDays(10);
        var driverLicense = new DriverLicense(numberLicense, licenseCategory, expirationDate);

        Assert.False(driverLicense.CanDrive(license));
    }

    [Theory]
    [InlineData(LicenseCategory.C)]
    [InlineData(LicenseCategory.AC)]
    [InlineData(LicenseCategory.D)]
    [InlineData(LicenseCategory.AD)]
    [InlineData(LicenseCategory.E)]
    [InlineData(LicenseCategory.AE)]
    public void CanDrive_withCanDriveTruckWithHigherDriverLicense_ReturnTrue(LicenseCategory license)
    {
        var numberLicense = "123456789";
        var licenseCategory = license;
        var expirationDate = DateTime.UtcNow.AddDays(10);
        var driverLicense = new DriverLicense(numberLicense, licenseCategory, expirationDate);

        Assert.True(driverLicense.CanDrive(LicenseCategory.C));
    }

    [Theory]
    [InlineData(LicenseCategory.None)]
    [InlineData(LicenseCategory.A)]
    [InlineData(LicenseCategory.AB)]
    [InlineData(LicenseCategory.C)]
    [InlineData(LicenseCategory.AC)]
    [InlineData(LicenseCategory.D)]
    [InlineData(LicenseCategory.AD)]
    public void CanDrive_withCanDriveTruckWithSmallerDriverLicense_ReturnFalse(LicenseCategory license)
    {
        var numberLicense = "123456789";
        var licenseCategory = license;
        var expirationDate = DateTime.UtcNow.AddDays(10);
        var driverLicense = new DriverLicense(numberLicense, licenseCategory, expirationDate);

        Assert.False(driverLicense.CanDrive(LicenseCategory.E));
    }
}
