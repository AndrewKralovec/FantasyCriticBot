# FantasyCriticBot
Discord bot for fantasy critic

## Setup
Todo:
Add token to project `dotnet user-secrets set "Bot:ClientID" "TOKEN_HERE"`  
Define a `UserSecretsId` element within a `PropertyGroup` of the `FantasyBot.csproj` file. The inner text of `UserSecretsId` is arbitrary, but is unique to the project. `<UserSecretsId>PROJECT_ID</UserSecretsId>`  

Set prefix in `appsettings.json`  

## Dependencies
Discord.Net  
Microsoft.Extensions.Configuration  
Microsoft.Extensions.Configuration.FileExtensions  
Microsoft.Extensions.Configuration.Json  
Microsoft.Extensions.Configuration.UserSecrets  

## Todo
Fill out readme