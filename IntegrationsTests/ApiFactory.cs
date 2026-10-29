using Api.Handlers;
using Application.Contracts.Abstractions;
using Infra.Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace IntegrationsTests;

public sealed class ApiFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection =
        new("Data Source=:memory:");

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        _connection.Open();

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<ZeloFrotaDbContext>();
            services.RemoveAll<IDbContextOptionsConfiguration<ZeloFrotaDbContext>>();

            services.AddDbContext<ZeloFrotaDbContext>(
                options => options.UseSqlite(_connection));

            var permissionAuthorization = new Mock<IPermissionAuthorization>();

            permissionAuthorization
                .Setup(x => x.HasPermissionAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(true);

            permissionAuthorization
                .Setup(x => x.HasPermissionAsync(
                    10,
                    It.IsAny<string>()))
                .ReturnsAsync(false);

            services.RemoveAll<IPermissionAuthorization>();

            services.AddSingleton<IPermissionAuthorization>(permissionAuthorization.Object);

        });

    }

    public async Task InitializeDatabaseAsync()
    {
        using var scope = Services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<ZeloFrotaDbContext>();

        await context.Database.EnsureCreatedAsync();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
            _connection.Dispose();
    }
}