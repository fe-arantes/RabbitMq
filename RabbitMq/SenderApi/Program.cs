using Microsoft.OpenApi.Models;
using SenderApi.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMqConfig"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoolMessages.App", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoolMessages.App v1"));
}

app.UseAuthorization();

app.MapControllers();

app.Run();
