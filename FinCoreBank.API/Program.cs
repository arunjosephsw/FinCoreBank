using System.Text;
using FinCoreBank.API.Middlewares;
using FinCoreBank.Application.Interfaces.Repositories;
using FinCoreBank.Application.Interfaces.Services;
using FinCoreBank.Application.Services;
using FinCoreBank.Infrastructure.Data;
using FinCoreBank.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
 builder.Services.AddOpenApi();

builder.Services
.AddAuthentication(
JwtBearerDefaults.AuthenticationScheme)

.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,

            ValidateAudience = true,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,

            ValidIssuer =
            builder.Configuration["Jwt:Issuer"],

            ValidAudience =
            builder.Configuration["Jwt:Audience"],

            IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                builder.Configuration["Jwt:Key"]!))
        };
});

builder.Services.AddDbContext<BankingDbContext>(
options =>
options.UseSqlServer(
builder.Configuration
.GetConnectionString("cs")));

builder.Services.AddScoped<
ITransferRepository,
TransferRepository>();

builder.Services.AddScoped<
IFraudCheckService,
FraudCheckService>();

builder.Services.AddScoped<
IJwtService,
JwtService>();

builder.Services.AddScoped<
ITransferService,
TransferService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
     app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<
ExceptionMiddleware>();

app.UseAuthentication();

 app.UseAuthorization();

app.MapControllers();
// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast");

app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
