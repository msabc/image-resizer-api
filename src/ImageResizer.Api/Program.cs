using System.Text;
using ImageResizer.Api.Constants;
using ImageResizer.Api.Filters;
using ImageResizer.Api.OpenAPI;
using ImageResizer.Api.Services;
using ImageResizer.Domain.Interfaces.Services;
using ImageResizer.Domain.Models.Tables;
using ImageResizer.Infrastructure.DatabaseContext;
using ImageResizer.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;

const string ApplicationName = "ImageResizer.Api";

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information($"{ApplicationName} starting.");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ApiExceptionFilterAttribute>();
    });

    builder.Services.AddRouting(options =>
    {
        options.LowercaseUrls = true;
    });

    builder.Services.AddOpenApi(options =>
    {
        options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
    });

    var resizerSettings = builder.Services.RegisterApplicationDependencies(builder.Configuration);

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

    // Add Authorization
    builder.Services.AddAuthorization();

    // Authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var jwtSettings = resizerSettings.JWTSettings;

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.IssuerSigningKey))
        };
    });

    // Identity
    builder.Services
        .AddIdentityCore<ApplicationUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ResizerDbContext>()
        .AddDefaultTokenProviders();

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    });

    builder.Logging.ClearProviders();

    builder.Services.AddSerilog((services, loggingConfiguration) =>
    {
        loggingConfiguration.ReadFrom.Configuration(builder.Configuration)
                            .Enrich.FromLogContext();
    });

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ResizerDbContext>();
        dbContext.Database.EnsureCreated();
    }

    app.UseSerilogRequestLogging();

    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithPreferredScheme(Authentication.BearerAuthenticationSchemeName)
            .WithHttpBearerAuthentication(bearer =>
            {
                bearer.Token = "your-bearer-token";
            });

        options.Theme = ScalarTheme.DeepSpace;
    });

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

    Log.Information($"{ApplicationName} started.");
}
catch (Exception ex)
{
    Log.Fatal($"{ApplicationName} terminated due to a fatal exception: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
}
finally
{
    await Log.CloseAndFlushAsync();
}