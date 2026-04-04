using DotNetEnv;
using FeatureFolio.API.Extensions;
using FeatureFolio.Domain;
using Serilog;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Use serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

// Add services to the container.
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddSettingsOptions(builder.Configuration);
builder.Services.AddAzureServices(builder.Configuration);
builder.Services.AddServices(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddDevelopmentCors();
builder.Services.AddJwt(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors(Constants.DEV_CORS);

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.MapHealthChecks("/healthcheck");

app.Run();
