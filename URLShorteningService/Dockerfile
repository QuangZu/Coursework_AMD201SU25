# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "URLShorteningService.csproj"
RUN dotnet build "URLShorteningService.csproj" -c Release -o /app/build
RUN dotnet publish "URLShorteningService.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "URLShorteningService.dll"]
