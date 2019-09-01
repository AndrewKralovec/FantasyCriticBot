using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FantasyBot.Models
{
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
        public GameJson NextGameReleased() => games
               .Where(game => game.releaseDate > DateTime.Now)
               .OrderBy(game => game.releaseDate)
               .FirstOrDefault();
    }
}
