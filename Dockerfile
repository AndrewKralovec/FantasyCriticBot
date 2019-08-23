# FROM mcr.microsoft.com/dotnet/core/runtime:2.2
# COPY FantasyBot/bin/Release/netcoreapp2.2/publish/ FantasyBot/
# ENTRYPOINT ["dotnet", "FantasyBot/FantasyBot.dll"]

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /FantasyBot

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:2.2
WORKDIR /FantasyBot
COPY --from=build /FantasyBot/out .

ENTRYPOINT ["dotnet", "FantasyBot.dll"]
