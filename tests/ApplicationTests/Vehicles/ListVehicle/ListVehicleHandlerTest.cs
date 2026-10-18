using Application.Contracts.Abstractions.Vehicles.Query;
using Application.DTO.Vehicle;
using Application.UseCases.Vehicles.ListVehicle;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApplicationTests.Vehicles.ListVehicle;

public class ListVehicleHandlerTest
{
    private readonly CancellationToken _cancellationToken = new();
    private readonly Mock<IVehicleQuery> _mockQuery;
    private readonly Mock<ILogger<ListVehicleHandler>> _loggerMock;

    public ListVehicleHandlerTest()
    {
        _cancellationToken = new CancellationToken();
        _mockQuery = new Mock<IVehicleQuery>();
        _loggerMock = new Mock<ILogger<ListVehicleHandler>>();
    }

    [Fact]
    public async Task Handler_WithAnyErrorToList_ReturnResultFailure()
    {
        var command = new ListVehicleQuery() { Page = 1, Take = 10 };
        var listVehicleHandler = new ListVehicleHandler(_mockQuery.Object, _loggerMock.Object);
        _mockQuery.Setup(repo => repo.AllAsync(It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(new Exception("Erro inesperado"));

        var result = await listVehicleHandler.Handle(command, _cancellationToken);

        Assert.NotNull(result.Error);
        _mockQuery.Verify(repo => repo.AllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task Handler_WithValidParams_ReturnResultSuccess()
    {
        var command = new ListVehicleQuery() { Page = 1, Take = 10 };
        var listVehicleHandler = new ListVehicleHandler(_mockQuery.Object, _loggerMock.Object);
        var validVehicles = new List<VehicleDTO>() { new(), new()};
        int totalItems = 2;
        _mockQuery.Setup(repo => repo.AllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((validVehicles, totalItems));

        var result = await listVehicleHandler.Handle(command, _cancellationToken);

        Assert.Null(result.Error);
        Assert.True(result.IsSuccess);
        _mockQuery.Verify(repo => repo.AllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }
}
