# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EmployeeCRUD.Api/EmployeeCRUD.Api.csproj", "EmployeeCRUD.Api/"]
COPY ["EmployeeCRUD.Application/EmployeeCRUD.Application.csproj", "EmployeeCRUD.Application/"]
COPY ["EmployeeCRUD.Domain/EmployeeCRUD.Domain.csproj", "EmployeeCRUD.Domain/"]
COPY ["EmployeeCRUD.Infrastructure/EmployeeCRUD.Infrastructure.csproj", "EmployeeCRUD.Infrastructure/"]
RUN dotnet restore "./EmployeeCRUD.Api/EmployeeCRUD.Api.csproj"
COPY . .
WORKDIR "/src/EmployeeCRUD.Api"
RUN dotnet build "./EmployeeCRUD.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EmployeeCRUD.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmployeeCRUD.Api.dll"]