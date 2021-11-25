FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet publish streamer -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build-env /app/streamer/out .

ENTRYPOINT ["dotnet", "streamer.dll"]
