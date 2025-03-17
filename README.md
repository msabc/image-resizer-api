<h1 align="center">Image Resizer</h1>

**Image Resizer** is a **.NET 9 Web API** that enables users to upload images and resize them. 

## Architecture

The API implements [**Domain-driven design**](https://en.wikipedia.org/wiki/Domain-driven_design)
and is divided into several projects (layers):

- Configuration
    - holds configuration classes used to implement the [options pattern](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-9.0)
- IoC
    - central place for the entire DI ([dependency inversion](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/architectural-principles#dependency-inversion)) setup
- ThumbnailProcessor
    - contains the **Azure Function entry point**
    - the function is based on a queue trigger that creates thumbnails of each uploaded image
- Api
    - contains the **API entry point**
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
- PostgreSQL
- Scalar - for Open API UI
- Serilog - for logging
  - configure logging to your liking by modifying the **appsettings.json** file using the instructions provided in [**Serilog.Settings.Configuration**](https://github.com/serilog/serilog-settings-configuration) repository.
- Moq
- xUnit

## Authorization and authentication
- ImageResizer uses ASP.NET Core Identity as the authentication provider
- ImageResizer uses JWT Bearer Authentication

## Run the API locally

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
    "DatabaseSettings": {
      "ConnectionString": "Server=localhost;Port=5432;Database=Resizer;User Id=[YOUR_POSTGRES_USER];Password=[YOUR_POSTGRES_PASSWORD];"
      },
    "JWTSettings": {
      "Issuer": "https://[YOUR_ISSUER]",
      "Audience": "https://[YOUR_AUDIENCE]",
      "IssuerSigningKey": "[YOUR_128_BIT_KEY]"
    }
  }
}
```

**Note:**
Whatever value you choose for the **ConnectionString**, a new database will be created by the application automatically (if it doesn't already exist).

Replace the values in brackets with your own values. 
You can generate a random 128 bit key [**here**](https://generate-random.org/encryption-key-generator?count=1&bytes=16&cipher=aes-256-cbc).

5. Run the project

## Run the Azure Function locally

The setup is similar to the API, except the setup of user secrets is a bit different.

1. Set **ImageResizer.ThumbnailProcessor** as the Startup project
2. Configure **user secrets** necessary for local development: 
    - Right-click on the **ImageResizer.ThumbnailProcessor** project and click 'Manage User Secrets'
    - a secrets.json file will be created
    - populate the file with the following values:

 ```javascript
{
    "ResizerSettings:DatabaseSettings:ConnectionString": "Server=localhost;Port=5432;Database=Resizer;User Id=[YOUR_POSTGRES_USER];Password=[YOUR_POSTGRES_PASSWORD];"
}
```
**Note:**
1. Make sure you add the secrets using colon (:) or double-underscore (__) [notation](https://learn.microsoft.com/en-us/azure/azure-functions/functions-app-settings#app-setting-considerations) or a  otherwise they won't be resolved.
2. The connection string can be different than the one used in the API, but then your thumbnails will be stored elsewhere.