﻿using ImageResizer.Application.Services.AccessToken;
using ImageResizer.Application.Services.Image;
using ImageResizer.Application.Services.User;
using ImageResizer.Application.Services.Validation;
using ImageResizer.Configuration;
using ImageResizer.Domain.Interfaces.DatabaseContext;
using ImageResizer.Domain.Interfaces.Repositories;
using ImageResizer.Infrastructure.DatabaseContext;
using ImageResizer.Infrastructure.Repositories;
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

            services.RegisterDatabaseConfiguration(settings)
                    .RegisterDbContext()
                    .RegisterRepositories()
                    .RegisterApplicationServices();

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
            if (string.IsNullOrEmpty(settings.DatabaseSettings.ResizerConnectionString))
                throw new ArgumentNullException(nameof(settings));

            services.AddDbContext<ResizerDbContext>(options =>
            {
                options.UseSqlServer(settings.DatabaseSettings.ResizerConnectionString);
            });

            return services;
        }

        private static IServiceCollection RegisterDbContext(this IServiceCollection services)
        {
            services.AddScoped<IResizerDbContext, ResizerDbContext>();

            return services;
        }

        private static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IImageRepository, ImageRepository>();

            return services;
        }

        private static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccessTokenService, AccessTokenService>();

            services.AddScoped<IImageService, ImageService>();

            // validation
            services.AddScoped<IValidationService, ValidationService>();

            return services;
        }
    }
}