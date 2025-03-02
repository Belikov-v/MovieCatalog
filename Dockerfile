FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY *.sln .
COPY MovieCatalogConsole/*.csproj ./MovieCatalogConsole/
COPY MovieCatalogLibrary/*.csproj ./MovieCatalogLibrary/
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "MovieCatalogConsole.dll"]