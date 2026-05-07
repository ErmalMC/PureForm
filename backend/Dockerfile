# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["PureForm.sln", "./"]
COPY ["PureForm.Application/PureForm.Application.csproj", "PureForm.Application/"]
COPY ["PureForm.Domain/PureForm.Domain.csproj", "PureForm.Domain/"]
COPY ["PureForm.Infrastructure/PureForm.Infrastructure.csproj", "PureForm.Infrastructure/"]
COPY ["PureForm.WebAPI/PureForm.WebAPI.csproj", "PureForm.WebAPI/"]
COPY ["PureForm.Tests/PureForm.Tests.csproj", "PureForm.Tests/"]

# Restore dependencies
RUN dotnet restore "PureForm.sln"

# Copy source code
COPY . .

# Build the application
RUN dotnet build "PureForm.sln" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "PureForm.WebAPI/PureForm.WebAPI.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Install wget for health checks
RUN apt-get update && apt-get install -y wget && rm -rf /var/lib/apt/lists/*

# Copy published app
COPY --from=publish /app/publish .

# Expose port (Render will override with PORT env variable)
EXPOSE 5000

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD wget --no-verbose --tries=1 --spider http://localhost:5000/health || exit 1

# Run the application
ENTRYPOINT ["dotnet", "PureForm.WebAPI.dll"]

