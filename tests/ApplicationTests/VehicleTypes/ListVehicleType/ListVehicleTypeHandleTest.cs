using Application.UseCases.VehicleTypes.ListVehicleType;
using Domain.Entities;
using Domain.Interfaces.Repository;
using Moq;

namespace ApplicationTests.VehicleTypes.ListVehicleType;

public class ListVehicleTypeHandleTest
{
    private readonly CancellationToken _cancellationToken = new();
    private readonly Mock<IVehicleTypeRepository> _mockRepository;

    public ListVehicleTypeHandleTest()
    {
        _cancellationToken = new CancellationToken();
        _mockRepository = new Mock<IVehicleTypeRepository>();
    }

    [Fact]
    public async Task Handler_WithAnyErrorToList_ReturnResultFailure()
    {
        var query = new ListVehicleTypeQuery() { Skip = 0, Take = 10 };
        var listVehicleTypeHandler = new ListVehicleTypeHandle(_mockRepository.Object);
        _mockRepository.Setup(repo => repo.AllAsync(It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(new Exception("Erro inesperado"));

        var result = await listVehicleTypeHandler.Handle(query, _cancellationToken);

        Assert.NotNull(result.Error);
        _mockRepository.Verify(repo => repo.AllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task Handler_WithValidParams_ReturnResultSuccess()
    {
        var query = new ListVehicleTypeQuery() { Skip = 0, Take = 10 };
        var listVehicleHandler = new ListVehicleTypeHandle(_mockRepository.Object);
        var validVehicleTypes = new List<VehicleType>();
        _mockRepository.Setup(repo => repo.AllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(validVehicleTypes);

        var result = await listVehicleHandler.Handle(query, _cancellationToken);

        Assert.Null(result.Error);
        Assert.True(result.IsSuccess);
        _mockRepository.Verify(repo => repo.AllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }
}
