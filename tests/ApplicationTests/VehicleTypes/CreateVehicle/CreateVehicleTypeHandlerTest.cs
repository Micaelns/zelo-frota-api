using Application.UseCases.VehicleTypes.CreateVehicleType;
using Domain.Entities;
using Domain.Interfaces.Repository;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApplicationTests.VehicleTypes.CreateVehicle;

public class CreateVehicleTypeHandlerTest
{
    private readonly CancellationToken _cancellationToken = new();
    private readonly Mock<IVehicleTypeRepository> _mockRepository;
    private readonly Mock<ILogger<CreateVehicleTypeHandler>> _loggerMock;

    public CreateVehicleTypeHandlerTest()
    {
        _cancellationToken = new CancellationToken();
        _mockRepository = new Mock<IVehicleTypeRepository>();
        _loggerMock = new Mock<ILogger<CreateVehicleTypeHandler>>();
    }

    [Fact]
    public async Task Handler_WithAnyErrorToSave_ReturnResultFailure()
    {
        var command = new CreateVehicleTypeCommand() { Name = "teste type" };
        var createVehicleTypeHandler = new CreateVehicleTypeHandler(_mockRepository.Object, _loggerMock.Object); 
        _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<VehicleType>())).ThrowsAsync(new Exception("Erro inesperado"));

        var result = await createVehicleTypeHandler.Handle(command, _cancellationToken);

        Assert.NotNull(result.Error);
        _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<VehicleType>()), Times.Once);
    }

    [Fact]
    public async Task Handler_WithValidParams_ReturnResultSuccess()
    {
        var command = new CreateVehicleTypeCommand() { Name="teste type" };
        var createVehicleTypeHandler = new CreateVehicleTypeHandler(_mockRepository.Object, _loggerMock.Object);
        
        var result = await createVehicleTypeHandler.Handle(command, _cancellationToken);

        Assert.Null(result.Error);
        Assert.True(result.IsSuccess);
        _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<VehicleType>()), Times.Once);
    }
}
