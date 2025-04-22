using ImageResizer.Application.Services.AccessToken;
using ImageResizer.Application.Services.Image;
using ImageResizer.Application.Services.Thumbnail;
using ImageResizer.Application.Services.User;
using ImageResizer.Application.Services.Validation;
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

namespace ImageResizer.IoC
{
    public static class CompositionRoot
    {
        public static ResizerSettings RegisterApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
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

        private static ResizerSettings RegisterSettings(this IServiceCollection services, IConfiguration configuration)
        {
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
            services.AddScoped<IFileUploadRepository, FileUploadRepository>();
            services.AddScoped<IThumbnailRepository, ThumbnailRepository>();

            return services;
        }

        private static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccessTokenService, AccessTokenService>();

            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IThumbnailService, ThumbnailService>();

            // validation
            services.AddScoped<IFileValidationService, FileValidationService>();

            return services;
        }

        private static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IImageBlobService, ImageBlobService>();
            services.AddScoped<IThumbnailBlobService, ThumbnailBlobService>();
            services.AddScoped<IImageProcessorService, ImageProcessorService>();
            services.AddScoped<IThumbnailQueueService, ThumbnailQueueService>();
            services.AddScoped<IFeatureFlagService, FeatureFlagService>();

            return services;
        }
    }
}