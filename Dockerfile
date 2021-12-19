FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet publish streamer -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
RUN apt-get update
RUN apt-get install -y apt-utils
RUN apt-get install -y libgdiplus
RUN apt-get install -y libc6-dev 
WORKDIR /app
COPY --from=build-env /app/streamer/out .
COPY streamer/streamer.xml ./

ENTRYPOINT ["dotnet", "streamer.dll"]
