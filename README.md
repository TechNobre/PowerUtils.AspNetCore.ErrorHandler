# PowerUtils.AspNetCore.ErrorHandler

![Logo](https://raw.githubusercontent.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/main/assets/logo/logo_128x128.png)

***Handler to standardize error responses***

![Tests](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/actions/workflows/tests.yml/badge.svg)
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
  - [IProblemFactory](#ErrorHandler.IProblemFactory)
  - [Customize problem link and problem title](#ErrorHandler.CustomizeLinkAndTitle)
    - [Add new custom status code](#ErrorHandler.CustomizeLinkAndTitle.AddNew)
    - [Change link and title for a specific status code](#ErrorHandler.CustomizeLinkAndTitle.Change)
- [Contribution](#contribution)
- [License](./LICENSE)
- [Changelog](./CHANGELOG.md)



## Support to <a name="support-to"></a>
- .NET 6.0
- .NET 5.0



## Dependencies <a name="dependencies"></a>

- Microsoft.AspNetCore.App [NuGet](https://www.nuget.org/packages/Microsoft.AspNetCore.App/)
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


#### IProblemFactory <a name="ErrorHandler.IProblemFactory"></a>
How to create a custom error problem details for example in a controller

```csharp
[ApiController]
[Route("home")]
public class HomeController : ControllerBase
{
    private readonly IProblemFactory _problemFactory;

    public ProblemFactoryController(IProblemFactory problemFactory)
        => _problemFactory = problemFactory;



    [HttpGet("call-1")]
    public IActionResult Call1()
        => _problemFactory.CreateProblemResult(
            detail: "detail",
            instance: "instance",
            statusCode: (int)HttpStatusCode.BadRequest,
            title: "title",
            type: "type",
            errors: new Dictionary<string, string>
            {
                ["Property1"] = "Error1",
                ["Property2"] = "Error2",
                ["Property3"] = "Error3",
            }
        );

    [HttpGet("call-2")]
    public IActionResult Call2()
        => new ObjectResult(_problemFactory.CreateProblem(
            detail: "detail",
            instance: "instance",
            statusCode: (int)HttpStatusCode.BadRequest,
            title: "title",
            type: "type",
            errors: new Dictionary<string, string>
            {
                ["Property1"] = "Error1",
                ["Property2"] = "Error2",
                ["Property3"] = "Error3",
            }
        ));
}
```


#### Customize problem link and problem title <a name="ErrorHandler.CustomizeLinkAndTitle"></a>
Exception mapping to status code and error codes

##### Add new custom status code <a name="ErrorHandler.CustomizeLinkAndTitle.AddNew"></a>
```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.ClientErrorMapping.Add(582, new()
            {
                Link = "CustomLink",
                Title = "CustomTitle"
            });
        });
    }
}
```

##### Change link and title for a specific status code <a name="ErrorHandler.CustomizeLinkAndTitle.Change"></a>

Add your customization after `services.AddErrorHandler();` because it will override the defaults status codes
```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddErrorHandler();
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.ClientErrorMapping[403].Link = "OverrideLink";
            options.ClientErrorMapping[403].Title = "OverrideTitle";
        });
    }
}
```



## Contribution <a name="contribution"></a>

If you have any questions, comments, or suggestions, please open an [issue](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/issues/new/choose) or create a [pull request](https://github.com/TechNobre/PowerUtils.AspNetCore.ErrorHandler/compare)