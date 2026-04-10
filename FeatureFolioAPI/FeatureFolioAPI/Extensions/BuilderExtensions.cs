using Azure.Identity;
using FeatureFolio.Application.Interfaces;
using FeatureFolio.Application.Interfaces.DB;
using FeatureFolio.Application.Services;
using FeatureFolio.Domain;
using FeatureFolio.Infrastructure.DB;
using FeatureFolio.Infrastructure.Options;
using FeatureFolio.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata;
using System.Text;

namespace FeatureFolio.API.Extensions;

public static class BuilderExtensions
{
    public static IServiceCollection AddSettingsOptions(this IServiceCollection services, IConfiguration config)
    {
        // Bind and validate the options
        services.AddOptions<AzureOptions>()
            .Bind(config.GetSection(AzureOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<RedisOptions>()
            .Bind(config.GetSection(RedisOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<AuthOptions>()
            .Bind(config.GetSection(AuthOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<JwtOptions>()
            .Bind(config.GetSection(JwtOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
    public static IServiceCollection AddAzureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<DefaultAzureCredential>();

        services.AddAzureClients(clientBuilder =>
        {
            var azureOptions = new AzureOptions();
            config.GetSection(AzureOptions.SectionName).Bind(azureOptions);

            // Apply DefaultAzureCredential to all registered clients
            clientBuilder.UseCredential(new DefaultAzureCredential());

            // Register Blob Storage
            var storageUrl = azureOptions.storageOptions.StorageUrl;
            if (!string.IsNullOrEmpty(storageUrl))
            {
                clientBuilder.AddBlobServiceClient(new Uri(storageUrl));
            }

            // Register Service Bus
            var serviceBusNamespace = azureOptions.serviceBusOptions.ServiceBusNamespace;
            if (!string.IsNullOrEmpty(serviceBusNamespace))
            {
                clientBuilder.AddServiceBusClientWithNamespace(serviceBusNamespace);
            }

            services.AddHealthChecks()
                // 1. Azure Blob Storage Health Check
                .AddAzureBlobStorage(
                    new Uri(azureOptions.storageOptions.StorageUrl),
                    new DefaultAzureCredential(),
                    name: "BlobStorageCheck")

                // 2. Azure Service Bus Queue Health Check
                .AddAzureServiceBusTopic(
                    azureOptions.serviceBusOptions.ServiceBusNamespace,
                    azureOptions.serviceBusOptions.ImagesTopicName, // Replace with your actual queue name
                    new DefaultAzureCredential(),
                    name: "ServiceBusCheck");
        });

        return services;
    }
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IMessagePublisher, MessagePublisher>();

        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IGoogleAuthService, GoogleAuthService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration config)
    {
        var redisOptions = new RedisOptions();
        config.GetSection(RedisOptions.SectionName).Bind(redisOptions);

        services.AddStackExchangeRedisCache(options =>
        {
            // "localhost:6379" points to your Docker container. 
            // Later in Kubernetes, you just change this string to "redis-master:6379"
            options.Configuration = $"{redisOptions.Configuration}:{redisOptions.Port}";
            options.InstanceName = redisOptions.KeyPrefix;
        });

        return services;
    }
    public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration config)
    {
        var jwtOptions = new JwtOptions();
        config.GetSection(JwtOptions.SectionName).Bind(jwtOptions);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.Secret))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies[Constants.AUTH_SESSION];
                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }
    public static IServiceCollection AddPostgresDb(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("PostgresDb");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());
        return services;
    }
    public static IServiceCollection AddDevelopmentCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(Constants.DEV_CORS, policy =>
            {
                policy.WithOrigins("http://localhost:5173")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        return services;
    }
}
