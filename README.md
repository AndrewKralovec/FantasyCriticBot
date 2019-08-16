# FantasyCriticBot
Discord bot for fantasy critic.

## Todo
- [ ] Fill out readme.
- [x] Setup basic bot with commands
- [x] Setup Http Client for the Fantasy Critic Service
- [ ] Setup the c#/Json Models 
- [ ] Populate service methods for Fantasy Critic Service
    - [ ] League info
    - [ ] Player info
    - [ ] Game info
    - [ ] Linking Players and Channel members
- [ ] Create a Notification service


## Setup
- The bot token is going to be stored using the secret management tool. To use user secrets, you must define a `UserSecretsId` element within a `PropertyGroup` of the `FantasyBot.csproj` file. The inner text of `UserSecretsId` is arbitrary, but is unique to the project.`<UserSecretsId>PROJECT_ID</UserSecretsId>`. Then use dotnet to set the secret.
- Set your bot prefix in `appsettings.json`.

# Start
```sh
> dotnet user-secrets set "Bot:ClientID" "TOKEN_HERE"
> dotnet restore
> dotnet run
```

## Dependencies
- Discord.Net
- Microsoft.Extensions.Configuration
- Microsoft.Extensions.Configuration.FileExtensions
- Microsoft.Extensions.Configuration.Json
- Microsoft.Extensions.Configuration.UserSecrets
- Newtonsoft.Json
