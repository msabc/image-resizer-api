using ImageResizer.Application.Services.Thumbnail;
using ImageResizer.Configuration;
using ImageResizer.Domain.Interfaces.DatabaseContext;
using ImageResizer.Domain.Interfaces.Repositories;
using ImageResizer.Domain.Interfaces.Services;
using ImageResizer.Domain.Interfaces.Transactions;
using ImageResizer.Infrastructure.DatabaseContext;
using ImageResizer.Infrastructure.Repositories;
using ImageResizer.Infrastructure.Services;
using ImageResizer.Infrastructure.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageResizer.ThumbnailProcessor
{
    public static class CompositionExtensions
    {
        public static ResizerSettings RegisterFunctionDependencies(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            var settings = services.RegisterSettings(configuration);

            var dbSettings = services.RegisterDatabaseSettings(configuration);

            services.RegisterDatabaseConfiguration(dbSettings)
                    .RegisterDbContext()
                    .RegisterDatabaseServices()
                    .RegisterRepositories()
                    .RegisterApplicationServices()
                    .RegisterInfrastructureServices();

            return settings;
        }

        private static ResizerSettings RegisterSettings(this IServiceCollection services, ConfigurationManager configuration)
        {
            configuration.AddUserSecrets<Program>();

            services.Configure<ResizerSettings>(options => configuration.GetSection(nameof(ResizerSettings)).Bind(options));

            var settings = new ResizerSettings();
            configuration.GetSection(nameof(ResizerSettings)).Bind(settings);

            return settings;
        }

        private static ConnectionStrings RegisterDatabaseSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConnectionStrings>(options => configuration.GetSection(nameof(ConnectionStrings)).Bind(options));

            var dbSettings = new ConnectionStrings();
            configuration.GetSection(nameof(ConnectionStrings)).Bind(dbSettings);

            return dbSettings;
        }

        private static IServiceCollection RegisterDatabaseConfiguration(this IServiceCollection services, ConnectionStrings connectionStrings)
        {
            services.AddDbContext<ResizerDbContext>(options =>
            {
                options.UseNpgsql(connectionStrings.ResizerDatabaseConnectionString);
            });

            return services;
        }

        private static IServiceCollection RegisterDbContext(this IServiceCollection services)
        {
            services.AddScoped<IResizerDbContext, ResizerDbContext>();

            return services;
        }

        private static IServiceCollection RegisterDatabaseServices(this IServiceCollection services)
        {
            services.AddScoped<IResizerTransactionExecutor, ResizerTransactionExecutor>();

            return services;
        }

        private static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IThumbnailRepository, ThumbnailRepository>();

            return services;
        }

        private static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IThumbnailService, ThumbnailService>();

            return services;
        }

        private static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IThumbnailBlobService, ThumbnailBlobService>();
            services.AddScoped<IImageProcessorService, ImageProcessorService>();
            services.AddScoped<IThumbnailQueueService, ThumbnailQueueService>();

            return services;
        }
    }
}
