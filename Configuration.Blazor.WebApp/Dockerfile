# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ConfigurationLibrary/*.csproj ./ConfigurationLibrary/
COPY Configuration.Blazor.WebApp/*.csproj ./Configuration.Blazor.WebApp/

RUN mkdir ConfigurationLibrary.Tests
RUN mkdir ConfigurationConsoleApp

COPY . .

WORKDIR /src/Configuration.Blazor.WebApp
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Runtime 
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Configuration.Blazor.WebApp.dll"]
