FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["HEJCareSuite.sln", "."]
COPY ["src/HEJCareSuite.Domain/HEJCareSuite.Domain.csproj", "src/HEJCareSuite.Domain/"]
COPY ["src/HEJCareSuite.Application/HEJCareSuite.Application.csproj", "src/HEJCareSuite.Application/"]
COPY ["src/HEJCareSuite.Infrastructure/HEJCareSuite.Infrastructure.csproj", "src/HEJCareSuite.Infrastructure/"]
COPY ["src/HEJCareSuite.Api/HEJCareSuite.Api.csproj", "src/HEJCareSuite.Api/"]
RUN dotnet restore "src/HEJCareSuite.Api/HEJCareSuite.Api.csproj"

COPY ["src/", "src/"]
RUN dotnet publish "src/HEJCareSuite.Api/HEJCareSuite.Api.csproj" \
    --configuration Release \
    --output /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "HEJCareSuite.Api.dll"]
