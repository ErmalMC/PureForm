# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy solution file
COPY *.sln ./

# Copy csproj files
COPY PureForm.WebAPI/*.csproj ./PureForm.WebAPI/
COPY PureForm.Application/*.csproj ./PureForm.Application/
COPY PureForm.Domain/*.csproj ./PureForm.Domain/
COPY PureForm.Infrastructure/*.csproj ./PureForm.Infrastructure/

# Restore
RUN dotnet restore

# Copy everything else
COPY . .

# Build and publish
RUN dotnet publish PureForm.WebAPI/PureForm.WebAPI.csproj -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "PureForm.WebAPI.dll"]