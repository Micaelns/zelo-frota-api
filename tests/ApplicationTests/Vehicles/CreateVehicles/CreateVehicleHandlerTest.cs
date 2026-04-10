using Application.Vehicles.CreateVehicles;
using Domain.Entities;
using Domain.Interfaces.Repository;
using Moq;

namespace ApplicationTests.Vehicles.CreateVehicles;

public class CreateVehicleHandlerTest
{
    private readonly CancellationToken _cancellationToken = new();
    private readonly Mock<IVehicleRepository> _vehicleMockRepository;
    private readonly Mock<IVehicleTypeRepository> _vehicleTypeMockRepository;

    public CreateVehicleHandlerTest()
    {
        _cancellationToken = new CancellationToken();
        _vehicleMockRepository = new Mock<IVehicleRepository>();
        _vehicleTypeMockRepository = new Mock<IVehicleTypeRepository>();
    }

    [Fact]
    public async Task Handler_WithInvalidPlate_ReturnResultFailure()
    {
        var command = new CreateVehicleCommand() { Plate = "ASS000", Type = new Guid() };
        var createVehicleHandler = new CreateVehicleHandler(_vehicleMockRepository.Object, _vehicleTypeMockRepository.Object);

        var result = await createVehicleHandler.Handle(command, _cancellationToken);

        Assert.NotNull(result.Error);
        _vehicleMockRepository.Verify(repo => repo.GetByPlateAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handler_WithExistsPlate_ReturnResultFailure()
    {
        var command = new CreateVehicleCommand() { Plate = "ASS0000", Type = new Guid() };
        var validVehicle = new Vehicle(Guid.NewGuid(), new("AAA1A23"), 10000);
        var createVehicleHandler = new CreateVehicleHandler(_vehicleMockRepository.Object, _vehicleTypeMockRepository.Object);
        _vehicleMockRepository.Setup(repo => repo.GetByPlateAsync(It.IsAny<string>())).ReturnsAsync(validVehicle);

        var result = await createVehicleHandler.Handle(command, _cancellationToken);

        Assert.Equal("Veículo já cadastrado", result.Error);
        _vehicleMockRepository.Verify(repo => repo.GetByPlateAsync(It.IsAny<string>()), Times.Once);
        _vehicleTypeMockRepository.Verify(repo => repo.FindAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Handler_WithNotFoundVehicleType_ReturnResultFailure()
    {
        var command = new CreateVehicleCommand() { Plate = "ASS0000", Type = new Guid() };
        var createVehicleHandler = new CreateVehicleHandler(_vehicleMockRepository.Object, _vehicleTypeMockRepository.Object);

        var result = await createVehicleHandler.Handle(command, _cancellationToken);

        Assert.Equal("Tipo de veículo não existe", result.Error);
        _vehicleTypeMockRepository.Verify(repo => repo.FindAsync(It.IsAny<Guid>()), Times.Once);
        _vehicleMockRepository.Verify(repo => repo.AddAsync(It.IsAny<Vehicle>()), Times.Never);
    }

    [Fact]
    public async Task Handler_WithAnyErrorToSave_ReturnResultFailure()
    {
        var command = new CreateVehicleCommand() { Plate = "ASS0000", Type = new Guid() };
        var validVehicleType = new VehicleType("TesteTipo");
        var createVehicleHandler = new CreateVehicleHandler(_vehicleMockRepository.Object, _vehicleTypeMockRepository.Object);
        _vehicleTypeMockRepository.Setup(repo => repo.FindAsync(It.IsAny<Guid>())).ReturnsAsync(validVehicleType);
        _vehicleMockRepository.Setup(repo => repo.AddAsync(It.IsAny<Vehicle>())).ThrowsAsync(new Exception("Erro inesperado"));

        var result = await createVehicleHandler.Handle(command, _cancellationToken);

        Assert.NotNull(result.Error);
        _vehicleMockRepository.Verify(repo => repo.AddAsync(It.IsAny<Vehicle>()), Times.Once);
    }

    [Fact]
    public async Task Handler_WithValidParams_ReturnResultSuccess()
    {
        var command = new CreateVehicleCommand() { Plate = "ASS0000", Type = new Guid() };
        var validVehicleType = new VehicleType("TesteTipo");
        var createVehicleHandler = new CreateVehicleHandler(_vehicleMockRepository.Object, _vehicleTypeMockRepository.Object);
        _vehicleTypeMockRepository.Setup(repo => repo.FindAsync(It.IsAny<Guid>())).ReturnsAsync(validVehicleType);

        var result = await createVehicleHandler.Handle(command, _cancellationToken);

        Assert.Null(result.Error);
        Assert.True(result.IsSuccess);
        _vehicleMockRepository.Verify(repo => repo.AddAsync(It.IsAny<Vehicle>()), Times.Once);
    }
}
