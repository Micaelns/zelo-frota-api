using Application.UseCases.Destinations.CreateDestination;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IntegrationsTests.Controllers;

public class DestinationControllerTests(ApiFactory factory) : IClassFixture<ApiFactory>, IAsyncLifetime
{
    private readonly ApiFactory _factory = factory;
    private HttpClient _client = null!;

    public async Task DisposeAsync()
    {
        _client.Dispose();
    }

    public async Task InitializeAsync()
    {
        await _factory.InitializeDatabaseAsync();
        _client = _factory.CreateClient();
    }

    private void AddTokenInHttpClient(int userId)
    {
        var configuration = _factory.Services.GetRequiredService<IConfiguration>();
        var token = TestTokenGenerator.Execute(configuration, userId);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    [Fact]
    public async Task Index_DefautParams_WithoutAutheticate_returnUnauthorized()
    {
        var response = await _client.GetAsync("api/Destination");
        
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Index_DefautParams_WithoutPermission_returnForbidden()
    {
        AddTokenInHttpClient(10);
        var response = await _client.GetAsync("api/Destination");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Index_DefautParams_WithPermission_returnResultOk()
    {
        AddTokenInHttpClient(1);

        var response = await _client.GetAsync("api/Destination");

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<Destination>>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result?.Pagination);
        Assert.Equal(result?.Pagination.CurrentPage, 1);
        Assert.Equal(result?.Pagination.PerPage, 10);
    }

    [Fact]
    public async Task Create_ValidParams_returnResultOk()
    {
        AddTokenInHttpClient(1);
        var DestinationCommand = new CreateDestinationCommand { City = "São Paulo", Uf = "SP",ZipCode = "97256012" };
        var response = await _client.PostAsJsonAsync("api/Destination", DestinationCommand);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<Guid?>>();

        Assert.True(result?.IsSuccess);
        Assert.NotNull(result?.Value);
        Assert.Null(result?.ErrorType);
    }

    [Fact]
    public async Task Create_withInvalidParams_returnErrorValidation()
    {
        AddTokenInHttpClient(1);
        var DestinationCommand = new CreateDestinationCommand { City = "São Paulo", Uf = "AA", ZipCode = "97256012" };
        var response = await _client.PostAsJsonAsync("api/Destination", DestinationCommand);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<Guid?>>();

        Assert.False(result?.IsSuccess);
        Assert.NotNull(result?.Error);
        Assert.Equal(result?.ErrorType, Application.DTO.ErrorType.Validation);
    }
}
