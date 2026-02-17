FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file first
COPY *.sln .

# Copy the project file
COPY AssetNex.API/*.csproj ./AssetNex.API/

# Restore ALL packages including Asp.Versioning
RUN dotnet restore AssetNex.API/AssetNex.API.csproj

# Copy everything else
COPY . .

# Publish
WORKDIR /src/AssetNex.API
RUN dotnet publish AssetNex.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:$PORT
EXPOSE $PORT

ENTRYPOINT ["dotnet", "AssetNex.API.dll"]