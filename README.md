<h1 align="center">Image Resizer</h1>

This project is created for learning purposes only.

**Image Resizer** is a **.NET 9 Web API** that enables users to upload images and resize them. 
The solution also contains a **ThumbnailProcessor** Azure Function which automatically creates a thumbnail based on each
uploaded image while keeping the original aspect ratio.

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
- Azure Queue Storage - for thumbnail processing
- Azure Blob Storage - for image storage
- EFCore 9 - Code first approach - for database access
- PostgreSQL - for data storage
- Scalar - for Open API UI
- ImageSharp - for image processing
- Serilog - for logging
  - configure logging to your liking by modifying the **appsettings.json** file using the instructions provided in [**Serilog.Settings.Configuration**](https://github.com/serilog/serilog-settings-configuration) repository.
- Moq
- xUnit
- Github Actions - for CI/CD

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
  "ConnectionStrings": {
		"ResizerDatabaseConnectionString": "[YOUR_POSTGRESQL_CONNECTION_STRING]",
		"BlobStorageConnectionString": "UseDevelopmentStorage=true",
		"QueueStorageConnectionString": "UseDevelopmentStorage=true"
	},
  "ResizerSettings": {
    "JWTSettings": {
      "Issuer": "https://[YOUR_ISSUER]",
      "Audience": "https://[YOUR_AUDIENCE]",
      "IssuerSigningKey": "[YOUR_128_BIT_KEY]"
    }
  }
}
```

**Note:**
> Replace the values in brackets with your own values. 
>> Whatever value you choose for the **ConnectionString**, a new database will be created by the application automatically (if it doesn't already exist).

You can generate a random 128 bit key [**here**](https://generate-random.org/encryption-key-generator?count=1&bytes=16&cipher=aes-256-cbc).

5. Run the project

## Run the Azure Function locally

1. Set **ImageResizer.ThumbnailProcessor** as the Startup project
2. Configure **user secrets** necessary for local development: 
    - Right-click on the **ImageResizer.ThumbnailProcessor** project and click 'Manage User Secrets'
    - a secrets.json file will be created
    - populate the file with the following values:

 ```javascript
{
    "ConnectionStrings": {
        "ResizerDatabaseConnectionString": "[YOUR_POSTGRESQL_CONNECTION_STRING]",
        "BlobStorageConnectionString": "UseDevelopmentStorage=true"
    }
}
```

## CI/CD

This repository uses Github Actions for CI/CD.

Currently there are two workflows defined in the  **.github/workflows** folder:

1. *build-and-test* 
    - doesn't deploy the application and is triggered **automatically**
2. *deploy* 
    - deploys the application and is triggered **manually**
    - requires resource provisioning