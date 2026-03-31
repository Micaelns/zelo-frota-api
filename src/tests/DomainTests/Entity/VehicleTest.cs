using Domain.Entities;
using Domain.ObjectValues;
using DomainTests.Mocks;
using Newtonsoft.Json.Linq;

namespace DomainTests.Entity;

public class VehicleTest
{
    [Fact]
    public void Vehicle_withCorrectParams_ReturnValidVehicle()
    {
        var id = 1;
        var type = new VehicleType(1,"Cavalinho Scanna");
        var plate = new Plate("ASD-1W56");

        var vehicle = new Vehicle(id, type, plate, 0, []);

        Assert.NotNull(vehicle);
        Assert.Equal(id, vehicle.Id);
        Assert.Equal(type, vehicle.Type);
        Assert.Equal(plate.Value, vehicle.Plate);
    }

    [Fact]
    public void NewMileage_InsertValidMileage_HaventError()
    {
        var assertNewMiliage = 10001;
        var vehicle = VehicleMock.ValidVehicle(mileage:10000);

        vehicle.NewMileage(assertNewMiliage);

        Assert.Equal(vehicle.Mileage, assertNewMiliage);
    }


    [Fact]
    public void NewMileage_InsertInValidMileage_HaveAError()
    {
        var assertNewMiliage = 1000;
        var vehicle = VehicleMock.ValidVehicle(mileage: 10000);

        var excecao = Assert.Throws<ArgumentException>(() => vehicle.NewMileage(assertNewMiliage));

        Assert.Equal("Quilometragem inválida", excecao.Message);
    }
}