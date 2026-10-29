using Application.UseCases.VehicleTypes.CreateVehicleType;
using Domain.Entities;
using Domain.Enum;
using Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IntegrationsTests.Controllers;

public class VehicleTypeControllerTests(ApiFactory factory) : IClassFixture<ApiFactory>, IAsyncLifetime
{
    private readonly ApiFactory _factory = factory;
    private HttpClient _client = null!;

    public async Task InitializeAsync()
    {
        await _factory.InitializeDatabaseAsync();
        await SeedVehicleTypesAsync(_factory, 10);
        _client = _factory.CreateClient();
    }

    public async Task DisposeAsync()
    {
        _client.Dispose();
    }

    private void AddTokenInHttpClient(int userId, HttpClient? client = null)
    {
        var configuration = _factory.Services.GetRequiredService<IConfiguration>();
        var token = TestTokenGenerator.Execute(configuration, userId);
        var httpClient = client ?? _client;
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    [Fact(DisplayName = "Index_DefautParams_returnResultOk[Ambiente isolado]")]
    public async Task Index_DefautParams_returnResultOk()
    {
        using var factory = new ApiFactory();
        await factory.InitializeDatabaseAsync();

        using var client = factory.CreateClient();

        AddTokenInHttpClient(1, client);

        var response = await client.GetAsync("api/VehicleType");

        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact(DisplayName = "Index_validParamsPaginate_returnPaginateData[Ambiente isolado]")]
    public async Task Index_validParamsPaginate_returnPaginateData()
    {
        using var factory = new ApiFactory();
        await factory.InitializeDatabaseAsync();
        int maxRange = 12;
        await SeedVehicleTypesAsync(factory, maxRange);

        using var client = factory.CreateClient();
        AddTokenInHttpClient(1, client);

        var response = await client.GetAsync("api/VehicleType?page=2&take=7");
        var content = await response.Content.ReadAsStringAsync();
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<VehicleType>>>();

        Assert.NotNull(result?.Pagination);
        Assert.Equal(result?.Pagination.TotalItems, maxRange);
        Assert.Equal(result?.Pagination.CurrentPage,2);
        Assert.Equal(result?.Pagination.PerPage, 7);
        Assert.Equal(result?.Pagination.TotalPages,2);
        Assert.Equal(result?.Value?.Count,5); //12-7
    }

    [Theory]
    [InlineData(-2, -2)]
    [InlineData(0, -3)]
    [InlineData(-3, 200)]
    [InlineData(0, 200)]
    public async Task Index_pageNegaiveOrZero_returnPaginateDataPage1AndPerPage1OR100(int page, int perPage)
    {
        AddTokenInHttpClient(1);
        var response = await _client.GetAsync("api/VehicleType?page=" + page + "&take=" + perPage);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<VehicleType>>>();

        Assert.NotNull(result?.Pagination);
        Assert.Equal(result?.Pagination.CurrentPage, 1);
        Assert.Equal(result?.Pagination.PerPage, Math.Clamp(perPage, 1, 100));
    }

    [Fact]
    public async Task Show_ValidParams_returnElement()
    {
        AddTokenInHttpClient(1);
        var vehicleType = await CreateVehicleTypesAsync();
        var response = await _client.GetAsync("api/VehicleType/"+vehicleType.Id);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<VehicleType>>();

        Assert.True(result?.IsSuccess);
        Assert.Null(result?.Pagination);
        Assert.Equal(result?.Value?.Name, vehicleType.Name);
    }

    [Fact]
    public async Task Show_InvalidParams_returnBadRequest()
    {
        AddTokenInHttpClient(1);
        var vehicleTypeId = Guid.NewGuid();
        var response = await _client.GetAsync("api/VehicleType/" + vehicleTypeId);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<VehicleType>>();

        Assert.False(result?.IsSuccess);
        Assert.Null(result?.Value);
        Assert.Equal(result?.ErrorType, Application.DTO.ErrorType.NotFound);
    }

    [Fact]
    public async Task Create_ValidParams_returnStatusCreated()
    {
        AddTokenInHttpClient(1);
        var vehicleType = new CreateVehicleTypeCommand { Name = "Created-VehicleType", RequeridLicenseCategory = LicenseCategory.AB };
        var response = await _client.PostAsJsonAsync("api/VehicleType", vehicleType);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<Guid?>>();

        Assert.True(result?.IsSuccess);
        Assert.NotNull(result?.Value);
        Assert.Null(result?.ErrorType);
    }

    [Fact]
    public async Task Create_InvalidParams_returnBadRequest()
    {
        AddTokenInHttpClient(1);
        var vehicleType = new CreateVehicleTypeCommand();
        var response = await _client.PostAsJsonAsync("api/VehicleType", vehicleType);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<Guid?>>();

        Assert.False(result?.IsSuccess);
        Assert.Null(result?.Value);
        Assert.Equal(result?.ErrorType, Application.DTO.ErrorType.Validation);
    }

    [Fact]
    public async Task Update_ValidParams_returnOK()
    {
        AddTokenInHttpClient(1);
        var vehicleTypeLocal = await CreateVehicleTypesAsync();
        var vehicleType = new CreateVehicleTypeCommand { Name = "Updated-VehicleType", RequeridLicenseCategory = LicenseCategory.AE };
        var response = await _client.PutAsJsonAsync("api/VehicleType/" + vehicleTypeLocal.Id, vehicleType);

        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Update_InvalidParams_returnBadRequest()
    {
        AddTokenInHttpClient(1);
        var vehicleTypeId = Guid.NewGuid();
        var vehicleType = new CreateVehicleTypeCommand();
        var response = await _client.PutAsJsonAsync("api/VehicleType/"+ vehicleTypeId, vehicleType);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<Guid?>>();

        Assert.False(result?.IsSuccess);
        Assert.Null(result?.Value);
        Assert.Equal(result?.ErrorType, Application.DTO.ErrorType.Validation);
    }

    [Fact]
    public async Task Delete_ValidParams_returnOK()
    {
        AddTokenInHttpClient(1);
        var vehicleTypeLocal = await CreateVehicleTypesAsync();
        var response = await _client.DeleteAsync("api/VehicleType/" + vehicleTypeLocal.Id);

        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Delete_InvalidParams_returnOK()
    {
        AddTokenInHttpClient(1);
        var vehicleTypeId = Guid.NewGuid();
        var response = await _client.DeleteAsync("api/VehicleType/" + vehicleTypeId);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<VehicleType>>();

        Assert.False(result?.IsSuccess);
        Assert.Null(result?.Value);
        Assert.Equal(result?.ErrorType, Application.DTO.ErrorType.NotFound);
    }

    private async Task SeedVehicleTypesAsync(ApiFactory factory, int maxRange)
    {
        using var scope = factory.Services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<ZeloFrotaDbContext>();

        var vehicleTypes = Enumerable.Range(1, maxRange)
            .Select(i => new VehicleType($"Type {i}"))
            .ToList();

        context.VehicleTypes.AddRange(vehicleTypes);

        await context.SaveChangesAsync();
    }

    private async Task<VehicleType> CreateVehicleTypesAsync()
    {
        using var scope = _factory.Services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<ZeloFrotaDbContext>();

        var qtd = await context.VehicleTypes.CountAsync();
        var vehicleTypes = new VehicleType($"New type {qtd}");

        await context.VehicleTypes.AddAsync(vehicleTypes);

        await context.SaveChangesAsync();
        return vehicleTypes;
    }
}
