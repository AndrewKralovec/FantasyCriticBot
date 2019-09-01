# FantasyCriticBot
Discord bot for fantasy critic.

# Quick-Start Guide

- [Setup](#setup)
- [Start](#start)
- [Usage](#usage)
- [Todo](#todo)
- [Dependencies](#dependencies)
- [Notes/Thoughts ](#notes)

## Setup
- The bot token is going to be stored using the secret management tool. To use user secrets, you must define a `UserSecretsId` element within a `PropertyGroup` of the `FantasyBot.csproj` file. The inner text of `UserSecretsId` is arbitrary, but is unique to the project.`<UserSecretsId>PROJECT_ID</UserSecretsId>`. Then use dotnet to set the secret. If this is not desirable, please feel free to set your secrets in `appsettings.json` or any other method.
- Set your bot prefix in `appsettings.json`. Please set up any config you want in `appsettings.json`... for now. 
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
    "Prefix": "!",
},
"User": {
    ...
}
```
- (Optional), create a docker image. 
```sh
> docker build -t bot_image -f Dockerfile .
```

## Start
```sh
> dotnet restore
> dotnet run
```

## Usage
Once but is running these are the current commands.

| Name  | Description |
| ------------- | ------------- |
| say  | Echos back the command you type. Used for testing.  |
| standings  | Get the league standings.  |
| next_release  | Get the next game that will be released for your league.  |


## Todo
- [x] Fill out readme.
- [x] Setup basic bot with commands.
- [x] Setup Http Client for the Fantasy Critic Service.
- [x] Setup the c#/Json Models.
- [ ] Populate service methods for Fantasy Critic Service.
    - [ ] League info.
    - [x] League standings info.
    - [x] Player info.
    - [x] Game release info.
    - [ ] Game searching.
    - [ ] Linking Players and Channel members.
	- [ ] Have multiple Leagues.
	- [ ] Handle the http client expiration time.
- [x] Setup Notification Service to notify discord members about league/game details.
- [ ] Populate service methods for Notification Service.
    - [x] Notify discord channel of league game releases.
    - [ ] Error preventing Notification Scheduling time.
    - [ ] Stop/Start Scheduled tasks.
    - [ ] Set which channel/users get the notification.
- [ ] Move classes into their own solutions.
- [ ] Testing.
- [x] Summary comments for class methods.
- [x] Setup Docker.

## Dependencies
- Discord.Net
- Microsoft.Extensions.Configuration
- Microsoft.Extensions.Configuration.FileExtensions
- Microsoft.Extensions.Configuration.Json
- Microsoft.Extensions.Configuration.UserSecrets
- Newtonsoft.Json

## Notes
Using dotnet because im more familiar with it. However, mono is a better supported runtime. Maybe i switch.  
Thinking that the notification Service should be in the same thread as the bot.  
The PostFix Json for the fantasy models, might be miss leadings, since they are read as json but parsed. Maybe i change.  
I feel that Notification Service will eventually become multiple services (if this project gets that far). So, making an abstract class for it.  
