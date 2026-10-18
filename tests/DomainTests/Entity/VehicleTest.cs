using Domain.Entities;
using Domain.Enum;
using Domain.ObjectValues;

namespace DomainTests.Entity;

public class VehicleTest
{
    [Fact]
    public void Vehicle_withCorrectParams_ReturnValidVehicle()
    {
        var type = Guid.NewGuid();
        var plate = new Plate("ASD-1W56");

        var vehicle = new Vehicle(type, plate, 0);

        Assert.NotNull(vehicle);
        Assert.Equal(type, vehicle.VehicleTypeId);
        Assert.Equal(plate.Value, vehicle.Plate);
    }

    [Fact]
    public void NewMileage_InsertValidMileage_HaventError()
    {
        var assertNewMiliage = 10001;
        var vehicle = new Vehicle(Guid.NewGuid(), new("AAA1A23"), 10000);

        vehicle.NewMileage(assertNewMiliage);

        Assert.Equal(vehicle.Mileage, assertNewMiliage);
    }


    [Fact]
    public void NewMileage_InsertInValidMileage_ReturnAArgumentException()
    {
        var assertNewMiliage = 1000;
        var vehicle = new Vehicle(Guid.NewGuid(), new("AAA1A23"), 10000);

        var excecao = Assert.Throws<ArgumentException>(() => vehicle.NewMileage(assertNewMiliage));

        Assert.Equal("Quilometragem invalida", excecao.Message);
    }

    [Fact]
    public void StartTravel_ValidsParams_ReturnAValidTravel()
    {
        var destinationId = Guid.NewGuid();
        var vehicleTypeId = Guid.NewGuid();
        var hasOpenTravel = false;
        var vehicle = new Vehicle(vehicleTypeId, new("AAA1A23"), 10000);

        var newTravel = vehicle.StartTravel(destinationId, hasOpenTravel);

        Assert.Equal(newTravel.DestinationId, destinationId);
        Assert.Equal(newTravel.VehicleId, vehicle.Id);
    }

    [Fact]
    public void StartTravel_withDriverValid_ReturnAValidTravel()
    {
        var destinationId = Guid.NewGuid();
        var vehicleTypeId = Guid.NewGuid();
        var driverLicense = new DriverLicense("", LicenseCategory.AE, DateTime.UtcNow.AddDays(5));
        var driver = new Driver("nameDriver", null, driverLicense);
        var hasOpenTravel = false;
        var vehicle = new Vehicle(vehicleTypeId, new("AAA1A23"), 10000);
        var VehicleType = new VehicleType("Carreta Bitrem") { Id = vehicleTypeId, RequeridLicenseCategory = LicenseCategory.E };
        vehicle.VehicleType = VehicleType;

        var newTravel = vehicle.StartTravel(destinationId, hasOpenTravel, driver);

        Assert.Equal(newTravel.DestinationId, destinationId);
        Assert.Equal(newTravel.VehicleId, vehicle.Id);
    }

    [Fact]
    public void StartTravel_WithCurrentTravel_ReturnException()
    {
        var destinationId = Guid.NewGuid();
        var vehicleTypeId = Guid.NewGuid();
        var hasOpenTravel = true;
        var vehicle = new Vehicle(vehicleTypeId, new("AAA1A23"), 10000);

        var excecao = Assert.Throws<InvalidOperationException>(() => vehicle.StartTravel(destinationId, hasOpenTravel));

        Assert.Equal("Já existe uma viagem em andamento", excecao.Message);
    }

    [Fact]
    public void StartTravel_WithDriverValidAndWithoutVehicleType_ReturnException()
    {
        var destinationId = Guid.NewGuid();
        var vehicleTypeId = Guid.NewGuid();
        var driverLicense = new DriverLicense("", LicenseCategory.D, DateTime.UtcNow.AddDays(5));
        var driver = new Driver("nameDriver", null, driverLicense);
        var hasOpenTravel = false;
        var vehicle = new Vehicle(vehicleTypeId, new("AAA1A23"), 10000);

        var excecao = Assert.Throws<InvalidDataException>(() => vehicle.StartTravel(destinationId, hasOpenTravel, driver));

        Assert.Equal("VehicleType é necessário para calcular licença do motorista", excecao.Message);
    }

    [Fact]
    public void StartTravel_WithDriverInValid_ReturnException()
    {
        var destinationId = Guid.NewGuid();
        var vehicleTypeId = Guid.NewGuid();
        var driverLicense = new DriverLicense("", LicenseCategory.AC, DateTime.UtcNow.AddDays(5));
        var driver = new Driver("nameDriver", null, driverLicense);
        var hasOpenTravel = false;
        var vehicle = new Vehicle(vehicleTypeId, new("AAA1A23"), 10000);
        var VehicleType = new VehicleType("Carreta Bitrem") { Id = vehicleTypeId, RequeridLicenseCategory = LicenseCategory.E };
        vehicle.VehicleType = VehicleType;

        var excecao = Assert.Throws<InvalidOperationException>(() => vehicle.StartTravel(destinationId, hasOpenTravel, driver));
        
        Assert.Contains("Motorista não possui qualificação necessária", excecao.Message);
    }

    [Fact]
    public void EndTravel_WithValidParams_ReturnTravel()
    {
        var destinationId = Guid.NewGuid();
        var vehicleId = Guid.NewGuid();
        var vehicleTypeId = Guid.NewGuid();
        var vehicle = new Vehicle(vehicleTypeId, new("AAA1A23"), 10000);
        var travel = vehicle.StartTravel(destinationId, false);

        var newTravel = vehicle.EndTravel(travel,101000,40);

        Assert.Equal(newTravel.DestinationId, destinationId);
        Assert.Equal(newTravel.VehicleId, vehicle.Id);
    }

    [Fact]
    public void EndTravel_WithValidFinishMileage_ReturnTravel()
    {
        var destinationId = Guid.NewGuid();
        var vehicleId = Guid.NewGuid();
        var vehicleTypeId = Guid.NewGuid();
        var vehicle = new Vehicle(vehicleTypeId, new("AAA1A23"), 100002);
        var travel = vehicle.StartTravel(destinationId, false);

        var excecao = Assert.Throws<ArgumentException>(() => vehicle.EndTravel(travel, 100001, 40));

        Assert.Equal("Quilometragem final inválida", excecao.Message);
    }
}