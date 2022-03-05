# PowerUtils.AspNetCore.ErrorHandler
Handler to standardize error responses

![Tests](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/actions/workflows/test-project.yml/badge.svg)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=TechNobre_PowerUtils.AspNetCore.ErrorHandler&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=TechNobre_PowerUtils.AspNetCore.ErrorHandler)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=TechNobre_PowerUtils.AspNetCore.ErrorHandler&metric=coverage)](https://sonarcloud.io/summary/new_code?id=TechNobre_PowerUtils.AspNetCore.ErrorHandler)

[![NuGet](https://img.shields.io/nuget/v/PowerUtils.AspNetCore.ErrorHandler.svg)](https://www.nuget.org/packages/PowerUtils.AspNetCore.ErrorHandler)
[![Nuget](https://img.shields.io/nuget/dt/PowerUtils.AspNetCore.ErrorHandler.svg)](https://www.nuget.org/packages/PowerUtils.AspNetCore.ErrorHandler)
[![License: MIT](https://img.shields.io/github/license/TechNobre/PowerUtils.AspNetCore.ErrorHandler.svg)](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/blob/main/LICENSE)



## Support to
- .NET 3.1 or more



## Features

- [ErrorHandler](#ErrorHandler)
  - [PropertyNamingPolicy](#ErrorHandler.PropertyNamingPolicy)
  - [ExceptionMappers](#ErrorHandler.ExceptionMappers)



## Documentation

### Dependencies

- Microsoft.AspNetCore.Diagnostics [NuGet](https://www.nuget.org/packages/Microsoft.AspNetCore.Diagnostics/)
- Microsoft.AspNetCore.Http.Extensions [NuGet](https://www.nuget.org/packages/Microsoft.AspNetCore.Http.Extensions/)
- Microsoft.AspNetCore.Mvc.Core [NuGet](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Core/)
- PowerUtils.Net.Primitives [NuGet](https://www.nuget.org/packages/PowerUtils.Net.Primitives/)
- PowerUtils.Text [NuGet](https://www.nuget.org/packages/PowerUtils.Text/)


### How to use

#### Install NuGet package
This package is available through Nuget Packages: https://www.nuget.org/packages/PowerUtils.AspNetCore.ErrorHandler

**Nuget**
```bash
Install-Package PowerUtils.AspNetCore.ErrorHandler
```

**.NET CLI**
```
dotnet add package PowerUtils.AspNetCore.ErrorHandler
```



### HashExtensions <a name="HashExtensions"></a>

ErrorHandler configuration

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddErrorHandler();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseErrorHandler();
    }
}
```


#### PropertyNamingPolicy <a name="ErrorHandler.PropertyNamingPolicy"></a>
**Options:**
- **Original**: _Do not format the property_;
- **CamelCase**: E.g. from `ClientName` to `clientName` **Default value**;
- **SnakeCase**: E.g. from `ClientName` to `client_name`;

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddErrorHandler(options =>
        {
            options.PropertyNamingPolicy = PropertyNamingPolicy.SnakeCase;
        });
    }
}
```


#### ExceptionMappers <a name="ErrorHandler.ExceptionMappers"></a>
Exception mapping to status code and error codes

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddErrorHandler(options =>
        {
            options.ExceptionMapper<ModelStatesException>(exception => (exception.Status, exception.Errors));
            options.ExceptionMapper<TimeoutException>(exception => StatusCodes.Status504GatewayTimeout);
        });
    }
}
```



## Contribution

*Help me to help others*



## LICENSE

[MIT](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/blob/main/LICENSE)



## Changelog

[Here](./CHANGELOG.md)
