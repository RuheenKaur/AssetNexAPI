FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY *.sln .
COPY AssetNex.API/*.csproj ./AssetNex.API/

# Restore with explicit NuGet source
RUN dotnet restore AssetNex.API/AssetNex.API.csproj --source https://api.nuget.org/v3/index.json

# Copy everything else
COPY . .

# Publish with restore
WORKDIR /src/AssetNex.API
RUN dotnet publish AssetNex.API.csproj -c Release -o /app/publish --source https://api.nuget.org/v3/index.json

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:$PORT
EXPOSE $PORT

ENTRYPOINT ["dotnet", "AssetNex.API.dll"]