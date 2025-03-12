<h1 align="center">Image Resizer</h1>

**Image Resizer** is a **.NET 9 Web API** that enables users to upload images and resize them. 

## Architecture

The API implements [**Domain-driven design**](https://en.wikipedia.org/wiki/Domain-driven_design)
and is divided into several projects (layers):

- Configuration
    - holds configuration classes used to implement the [options pattern](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-9.0)
- IoC
    - central place for the entire DI ([dependency inversion](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/architectural-principles#dependency-inversion)) setup
- Api
    - contains the main entry point
- Application
    - contains the business logic
- Domain
    - contains the domain models
- Infrastructure
    - contains the database context
- Tests
    - contains tests (only unit tests for now)

## Technology

The project features the following technologies:

- .NET 9 Web API
- EFCore 9 - Code first approach - for database access
- SQL Server
- Scalar - for Open API UI
- Serilog - for logging
  - configure logging to your liking by modifying the **appsettings.json** file using the instructions provided in [**Serilog.Settings.Configuration**](https://github.com/serilog/serilog-settings-configuration) repository.
- Moq
- xUnit

## Authorization and authentication
- ImageResizer uses ASP.NET Core Identity as the authentication provider
- ImageResizer uses JWT Bearer Authentication

## Run the project

1. Clone the project
2. Open the solution (**ImageResizer.Api.sln**) in Visual Studio
3. Set **ImageResizer.Api** as the Startup project
4. Configure necessary settings: 
    - ImageResizer will create a database if one doesn't exist based on the **ConnectionString** setting.
    - ImageResizer requires a 128 bit key for JWT signature validation based on the **IssuerSigningKey** setting.
6. Run the project