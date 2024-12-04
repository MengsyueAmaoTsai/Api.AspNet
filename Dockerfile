ARG DOTNET_VERSION=8.0

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build

WORKDIR /app

COPY . .

RUN dotnet restore ./RichillCapital.Api.sln --nologo
RUN dotnet publish ./src/RichillCapital.Api/RichillCapital.Api.csproj -c Release -o ./artifacts --no-restore --nologo

FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS runtime
WORKDIR /app

COPY --from=build ./app/artifacts ./

ENTRYPOINT [ "dotnet", "RichillCapital.Api.dll" ]