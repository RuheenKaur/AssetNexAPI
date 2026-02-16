FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything first
COPY . .

# Find the .csproj file and restore
RUN find . -name '*.csproj' -exec dotnet restore {} \;

# Build and publish (finds the project automatically)
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:$PORT
EXPOSE $PORT

# Start the DLL (make sure this matches your actual DLL name)
ENTRYPOINT ["dotnet", "AssetNex.API.dll"]