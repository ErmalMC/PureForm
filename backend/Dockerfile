FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution file
COPY *.sln ./

# Copy all csproj files
COPY PureForm.WebAPI/*.csproj ./PureForm.WebAPI/
COPY PureForm.Application/*.csproj ./PureForm.Application/
COPY PureForm.Domain/*.csproj ./PureForm.Domain/
COPY PureForm.Infrastructure/*.csproj ./PureForm.Infrastructure/

# Restore
RUN dotnet restore

# Copy everything else
COPY . .

# Publish
RUN dotnet publish PureForm.WebAPI/PureForm.WebAPI.csproj -c Release -o out

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}

ENTRYPOINT ["dotnet", "PureForm.WebAPI.dll"]