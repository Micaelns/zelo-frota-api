using Infra.Extensions;
using Serilog.Sinks.Grafana.Loki;
using Serilog;

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

builder.Services.Configure<Infra.Messaging.Kafka.KafkaSettings>(
    builder.Configuration.GetSection("Kafka")
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

app.UseAuthorization();

app.MapControllers();

app.Run();
