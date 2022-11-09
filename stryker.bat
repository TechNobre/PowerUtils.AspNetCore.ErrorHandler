dotnet tool restore
dotnet restore
dotnet build --no-restore
dotnet stryker -tp tests/PowerUtils.AspNetCore.ErrorHandler.Tests/PowerUtils.AspNetCore.ErrorHandler.Tests.csproj -p PowerUtils.AspNetCore.ErrorHandler.csproj --reporter cleartext --reporter html -o