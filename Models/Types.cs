using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FantasyBot
{
    public static class Constants
    {
        public const string AppSettings = "appsettings.json";
        public const string ConfigID = "Bot:ClientID";
        public const string ConfigPrefix = "Bot:Prefix";
        public const string Dev = "development";
        public const string Env = "NETCORE_ENVIRONMENT";
        public const string JsonContent = "application/json";
    }

    public class GameJson
    {
        public Guid publisherGameID { get; set; }
        public string gameName { get; set; }
        public DateTime? timestamp { get; set; }
        public bool? counterPick { get; set; }
        public string estimatedReleaseDate { get; set; }
        public DateTime? releaseDate { get; set; }
        public double? fantasyPoints { get; set; }
        public double? criticScore { get; set; }
        public double simpleProjectedFantasyPoints { get; set; }
        public double advancedProjectedFantasyPoints { get; set; }
        public object masterGame { get; set; }
        public bool linked { get; set; }
        public bool? released { get; set; }
        public bool willRelease { get; set; }
        public bool manualCriticScore { get; set; }
    }

    public class PlayerJson
    {
        public Guid inviteID { get; set; }
        [JsonIgnore]
        public object inviteName { get; set; }
        public PublisherJson publisher { get; set; }
        public UserJson user { get; set; }
        public double totalFantasyPoints { get; set; }
        public double simpleProjectedFantasyPoints { get; set; }
        public double advancedProjectedFantasyPoints { get; set; }
    }

    public class PublisherJson
    {
        public Guid publisherID { get; set; }
        public Guid leagueID { get; set; }
        public string publisherName { get; set; }
        public string leagueName { get; set; }
        public string playerName { get; set; }
        public int year { get; set; }
        public int draftPosition { get; set; }
        public List<GameJson> games { get; set; }
        public double averageCriticScore { get; set; }
        public double totalFantasyPoints { get; set; }
        public double totalProjectedPoints { get; set; }
        public int budget { get; set; }
        public bool nextToDraft { get; set; }
        public bool userIsInLeague { get; set; }
        public bool publicLeague { get; set; }
        public bool outstandingInvite { get; set; }
    }
    public class UserJson
    {
        public Guid leagueID { get; set; }
        public string leagueName { get; set; }
        public string userID { get; set; }
        public string displayName { get; set; }
    }
}