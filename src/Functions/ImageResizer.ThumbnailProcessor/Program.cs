using ImageResizer.ThumbnailProcessor;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.RegisterFunctionDependencies(builder.Configuration);

builder.Build().Run();
