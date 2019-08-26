FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /FantasyCriticBot

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime
WORKDIR /FantasyCriticBot
COPY --from=build /FantasyCriticBot/out ./

ENTRYPOINT ["dotnet", "FantasyBot.dll"]