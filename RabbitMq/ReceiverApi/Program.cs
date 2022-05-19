using ReceiverApi.Consumers;
using ReceiverApi.Options;
using ReceiverApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMqConfig"));
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddHostedService<ProcessMessageConsumer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
