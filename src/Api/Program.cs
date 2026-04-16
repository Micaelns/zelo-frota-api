using Infra.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Infra.Messaging.Kafka.KafkaSettings>(
    builder.Configuration.GetSection("Kafka")
);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistence();
builder.Services.ImplementsRepository();
builder.Services.ImplementsServices();
builder.Services.RegisterMediatRUseCases();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
