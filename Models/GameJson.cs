using System;

namespace FantasyBot.Models
{
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
}