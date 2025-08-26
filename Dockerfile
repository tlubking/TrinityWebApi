# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj and restore first for better layer caching
COPY TrinityWebApi/TrinityWebApi.csproj TrinityWebApi/
RUN dotnet restore TrinityWebApi/TrinityWebApi.csproj

# copy the rest and publish
COPY . .
RUN dotnet publish TrinityWebApi/TrinityWebApi.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

# Default to Production in containers
ENV DOTNET_ENVIRONMENT=Production

# Copy published output
COPY --from=build /app/publish ./

# Bind to Railway's PORT if provided, otherwise 8080.
# Using sh -c to expand ${PORT} at runtime.
CMD ["sh", "-c", "ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080} dotnet TrinityWebApi.dll"]