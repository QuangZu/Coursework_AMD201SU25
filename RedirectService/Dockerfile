# Use the official .NET 8 SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY . .

RUN dotnet restore "./RedirectService.csproj"
RUN dotnet build "./RedirectService.csproj" -c Release -o /app/build
RUN dotnet publish "./RedirectService.csproj" -c Release -o /app/publish

# Final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "RedirectService.dll"]
