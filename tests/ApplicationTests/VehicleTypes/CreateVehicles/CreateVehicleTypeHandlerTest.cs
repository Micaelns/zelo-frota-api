using Application.VehicleTypes.CreateVehicleTypes;
using Domain.Entities;
using Domain.Interfaces.Repository;
using Moq;

namespace ApplicationTests.VehicleTypes.CreateVehicles;

public class CreateVehicleTypeHandlerTest
{
    private readonly CancellationToken _cancellationToken = new();
    private readonly Mock<IVehicleTypeRepository> _mockRepository;

    public CreateVehicleTypeHandlerTest()
    {
        _cancellationToken = new CancellationToken();
        _mockRepository = new Mock<IVehicleTypeRepository>();
    }

    [Fact]
    public async Task Handler_WithAnyErrorToSave_ReturnResultFailure()
    {
        var command = new CreateVehicleTypeCommand() { Name = "teste type" };
        var createVehicleTypeHandler = new CreateVehicleTypeHandler(_mockRepository.Object); 
        _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<VehicleType>())).ThrowsAsync(new Exception("Erro inesperado"));

        var result = await createVehicleTypeHandler.Handle(command, _cancellationToken);

        Assert.NotNull(result.Error);
        _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<VehicleType>()), Times.Once);
    }

    [Fact]
    public async Task Handler_WithValidParams_ReturnResultSuccess()
    {
        var command = new CreateVehicleTypeCommand() { Name="teste type" };
        var createVehicleTypeHandler = new CreateVehicleTypeHandler(_mockRepository.Object);
        
        var result = await createVehicleTypeHandler.Handle(command, _cancellationToken);

        Assert.Null(result.Error);
        Assert.True(result.IsSuccess);
        _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<VehicleType>()), Times.Once);
    }
}
