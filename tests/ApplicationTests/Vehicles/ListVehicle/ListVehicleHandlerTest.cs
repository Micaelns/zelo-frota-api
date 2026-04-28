using Application.UseCases.Vehicles.ListVehicle;
using Domain.Entities;
using Domain.Interfaces.Repository;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApplicationTests.Vehicles.ListVehicle;

public class ListVehicleHandlerTest
{
    private readonly CancellationToken _cancellationToken = new();
    private readonly Mock<IVehicleRepository> _mockRepository;
    private readonly Mock<ILogger<ListVehicleHandler>> _loggerMock;

    public ListVehicleHandlerTest()
    {
        _cancellationToken = new CancellationToken();
        _mockRepository = new Mock<IVehicleRepository>();
        _loggerMock = new Mock<ILogger<ListVehicleHandler>>();
    }

    [Fact]
    public async Task Handler_WithAnyErrorToList_ReturnResultFailure()
    {
        var command = new ListVehicleQuery() { Skip = 0, Take = 10 };
        var listVehicleHandler = new ListVehicleHandler(_mockRepository.Object, _loggerMock.Object);
        _mockRepository.Setup(repo => repo.AllAsync(It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(new Exception("Erro inesperado"));

        var result = await listVehicleHandler.Handle(command, _cancellationToken);

        Assert.NotNull(result.Error);
        _mockRepository.Verify(repo => repo.AllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task Handler_WithValidParams_ReturnResultSuccess()
    {
        var command = new ListVehicleQuery() { Skip = 0, Take = 10 };
        var listVehicleHandler = new ListVehicleHandler(_mockRepository.Object, _loggerMock.Object);
        var validVehicles = new List<Vehicle>();
        _mockRepository.Setup(repo => repo.AllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(validVehicles);

        var result = await listVehicleHandler.Handle(command, _cancellationToken);

        Assert.Null(result.Error);
        Assert.True(result.IsSuccess);
        _mockRepository.Verify(repo => repo.AllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }
}
