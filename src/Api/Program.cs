using Infra.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
{
    config
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}"
        );
});

builder.Services.Configure<Infra.Messaging.Kafka.KafkaSettings>(
    builder.Configuration.GetSection("Kafka")
);
// Add services to the container.

builder.Services.AddControllers();
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

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("PermitirLocalhost");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
