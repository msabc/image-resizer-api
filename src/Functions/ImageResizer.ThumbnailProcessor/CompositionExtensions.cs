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
using Microsoft.Extensions.Hosting;

namespace ImageResizer.ThumbnailProcessor
{
    public static class CompositionExtensions
    {
        public static ResizerSettings RegisterFunctionDependencies(
            this IServiceCollection services, 
            IConfiguration configuration, 
            IHostEnvironment hostEnvironment)
        {
            var settings = services.RegisterSettings(configuration);

            services.RegisterDatabaseConfiguration(settings)
                    .RegisterDbContext()
                    .RegisterDatabaseServices()
                    .RegisterRepositories()
                    .RegisterApplicationServices()
                    .RegisterInfrastructureServices();

            if (hostEnvironment.IsDevelopment())
                configuration.AddUserSecrets<Program>();

            return settings;
        }

        private static ResizerSettings RegisterSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ResizerSettings>(options => configuration.GetSection(nameof(ResizerSettings)).Bind(options));

            var settings = new ResizerSettings();
            configuration.GetSection(nameof(ResizerSettings)).Bind(settings);

            return settings;
        }

        private static IServiceCollection RegisterDatabaseConfiguration(this IServiceCollection services, ResizerSettings settings)
        {
            if (string.IsNullOrEmpty(settings.DatabaseSettings.ConnectionString))
                throw new ArgumentNullException(nameof(settings));

            services.AddDbContext<ResizerDbContext>(options =>
            {
                options.UseNpgsql(settings.DatabaseSettings.ConnectionString);
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
