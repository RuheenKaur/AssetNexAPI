FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY *.sln .
COPY AssetNex.API/*.csproj ./AssetNex.API/
RUN dotnet restore AssetNex.API/AssetNex.API.csproj

COPY . .

WORKDIR /src/AssetNex.API
RUN dotnet publish AssetNex.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:$PORT
EXPOSE $PORT

ENTRYPOINT ["dotnet", "AssetNex.API.dll"]