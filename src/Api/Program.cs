using Api.Handlers;
using Api.Providers;
using Infra.Cache;
using Infra.Cache.Mongo.context;
using Infra.Data.Commands;
using Infra.Data.Contexts;
using Infra.Extensions;
using Infra.Messaging.Kafka;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Serilog.Sinks.Grafana.Loki;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
{
    config
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.WithProperty("service_name", "ZeloFrota.Api")
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}"
        )
        .WriteTo.GrafanaLoki("http://localhost:3100", labels: [new LokiLabel("service_name", "ZeloFrota.Api")]);
});

builder.Services.Configure<KafkaSettings>(
    builder.Configuration.GetSection("Kafka")
);

builder.Services.Configure<CacheSettings>(
    builder.Configuration.GetSection("Cache")
);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenTelemetry(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddContexts(builder.Configuration["connectionStringSqlServer"]);

builder.Services.ImplementsRepository();
builder.Services.ImplementsServices();
builder.Services.RegisterMediatRUseCases(builder.Configuration["MediatRLicenseKey"]);
builder.Services.RegistryAuthenticRefit(builder.Configuration);
builder.Services.AddInfrastructureJWT(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler,CustomAuthorizationMiddlewareResultHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider,PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler,PermissionAuthorizationHandler>();
builder.Services.RegisterMongoCache(builder.Configuration);

builder.Services.AddCors(options =>
{
    var origins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [];

    options.AddPolicy("PermitirLocalhost", 
        policy => policy.WithOrigins(origins)
                .AllowAnyHeader()
                .AllowAnyMethod());
});
var app = builder.Build();

Console.WriteLine("*** Iniciando configurações da Aplicação: ");

if (args.Length > 0)
{
    using var scope = app.Services.CreateScope();

    var context = scope.ServiceProvider
        .GetRequiredService<ZeloFrotaDbContext>();

    switch (args[0])
    {
        case "seed:create":
            Console.WriteLine("- Executando Seeds: ");
            await CreateSeedCommand.ExecuteAsync(context);
            return;

        case "seed:remove":
            Console.WriteLine("- Removendo Seeds: ");
            await RemoveSeedCommand.ExecuteAsync(context);
            return;
    }
}

if (builder.Configuration["Cache:Provider"] == "Mongo")
{
    using var scope = app.Services.CreateScope();

    var context = scope.ServiceProvider
        .GetRequiredService<AuthorizationMongoContext>();

    await context.CreateIndexesAsync();
}
//app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("PermitirLocalhost");

app.MapGet("/", (IHostEnvironment env) =>
{
    return Results.Ok(new
    {
        application = "Zelo Frota Api",
        environment = env.EnvironmentName,
        timestamp = DateTime.UtcNow
    });
});
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
