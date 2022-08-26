# PowerUtils.AspNetCore.ErrorHandler

![Logo](https://raw.githubusercontent.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/main/assets/logo/logo_128x128.png)

***Handler to standardize error responses***

![Tests](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/actions/workflows/test-project.yml/badge.svg)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=TechNobre_PowerUtils.AspNetCore.ErrorHandler&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=TechNobre_PowerUtils.AspNetCore.ErrorHandler)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=TechNobre_PowerUtils.AspNetCore.ErrorHandler&metric=coverage)](https://sonarcloud.io/summary/new_code?id=TechNobre_PowerUtils.AspNetCore.ErrorHandler)

[![NuGet](https://img.shields.io/nuget/v/PowerUtils.AspNetCore.ErrorHandler.svg)](https://www.nuget.org/packages/PowerUtils.AspNetCore.ErrorHandler)
[![Nuget](https://img.shields.io/nuget/dt/PowerUtils.AspNetCore.ErrorHandler.svg)](https://www.nuget.org/packages/PowerUtils.AspNetCore.ErrorHandler)
[![License: MIT](https://img.shields.io/github/license/TechNobre/PowerUtils.AspNetCore.ErrorHandler.svg)](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/blob/main/LICENSE)


- [Support](#support-to)
- [Dependencies](#dependencies)
- [How to use](#how-to-use)
  - [Install NuGet package](#Installation)
  - [Configure](#ErrorHandler.Configure)
  - [PropertyNamingPolicy](#ErrorHandler.PropertyNamingPolicy)
  - [ExceptionMappers](#ErrorHandler.ExceptionMappers)
- [Contribution](#contribution)
- [License](./LICENSE)
- [Changelog](./CHANGELOG.md)



## Support to <a name="support-to"></a>
- .NET 6.0
- .NET 5.0
- .NET 3.1



## Dependencies <a name="dependencies"></a>

- Microsoft.AspNetCore.Diagnostics [NuGet](https://www.nuget.org/packages/Microsoft.AspNetCore.Diagnostics/)
- Microsoft.AspNetCore.Mvc.Core [NuGet](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Core/)
- PowerUtils.Net.Primitives [NuGet](https://www.nuget.org/packages/PowerUtils.Net.Primitives/)
- PowerUtils.Text [NuGet](https://www.nuget.org/packages/PowerUtils.Text/)



## How to use <a name="how-to-use"></a>

### Install NuGet package <a name="Installation"></a>
This package is available through Nuget Packages: https://www.nuget.org/packages/PowerUtils.AspNetCore.ErrorHandler

**Nuget**
```bash
Install-Package PowerUtils.AspNetCore.ErrorHandler
```

**.NET CLI**
```
dotnet add package PowerUtils.AspNetCore.ErrorHandler
```

### Configure <a name="ErrorHandler.Configure"></a>

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



## Contribution <a name="contribution"></a>

If you have any questions, comments, or suggestions, please open an [issue](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/issues/new/choose) or create a [pull request](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/compare)