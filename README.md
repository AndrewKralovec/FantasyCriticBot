# FantasyCriticBot
Discord bot for fantasy critic.

## Todo
- [ ] Fill out readme.
- [x] Setup basic bot with commands.
- [x] Setup Http Client for the Fantasy Critic Service.
- [x] Setup the c#/Json Models.
- [ ] Populate service methods for Fantasy Critic Service.
    - [ ] League info.
    - [x] League standings info.
    - [ ] Player info.
    - [ ] Game info.
    - [ ] Linking Players and Channel members.
	- [ ] Have multiple Leagues.
	- [ ] Handle the http client expiration time.
- [ ] Create a Notification service to notify discord members about league/game details.
- [ ] Move classes into their own solutions.
- [ ] Testing.
- [ ] Summary comments for class methods.

## Setup
- The bot token is going to be stored using the secret management tool. To use user secrets, you must define a `UserSecretsId` element within a `PropertyGroup` of the `FantasyBot.csproj` file. The inner text of `UserSecretsId` is arbitrary, but is unique to the project.`<UserSecretsId>PROJECT_ID</UserSecretsId>`. Then use dotnet to set the secret.
- Set your bot prefix in `appsettings.json`.
```sh
> dotnet user-secrets set "Bot:ClientID" "TOKEN_HERE"
```

`FantasyBot.csproj`   
```
<PropertyGroup>
<OutputType>Exe</OutputType>
<TargetFramework>netcoreapp2.1</TargetFramework>
<UserSecretsId>PROJECT_ID</UserSecretsId>
</PropertyGroup>
```

`appsettings.json`   
```
"Bot": {
    "Prefix": "!"
}
```

## Start
```sh
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

## Usage
Once but is running these are the current commands.  
- say: Echos back the command you type. Used for testing.
- standings: Leagues standings.

## Notes/Thoughts 
Using dotnet because im more familiar with it. However, mono is a better supported runtime. Maybe i switch.
Thinking that the notification Service should be in the same thread as the bot. You'll have to figure out how to store
your login credits. I'm not at the point of storing them yet. 
