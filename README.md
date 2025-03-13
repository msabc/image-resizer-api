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
4. Configure **user secrets** necessary for local development: 
    - Right-click on the **ImageResizer.Api** project and click 'Manage User Secrets'
    - a secrets.json file will be created
    - populate the file with the following values:
 ```javascript
{
  "ResizerSettings": {
    "JWTSettings": {
      "Issuer": "https://[YOUR_ISSUER]",
      "Audience": "https://[YOUR_AUDIENCE]",
      "IssuerSigningKey": "[YOUR_128_BIT_KEY]"
    }
  }
}
```

Replace the values in brackets with your own values. 
You can generate a random 128 bit key [**here**](https://generate-random.org/encryption-key-generator?count=1&bytes=16&cipher=aes-256-cbc).

5. Run the project