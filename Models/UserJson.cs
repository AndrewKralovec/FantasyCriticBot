using System;

namespace FantasyBot.Models
{
    public class UserJson
    {
        public Guid leagueID { get; set; }
        public string leagueName { get; set; }
        public string userID { get; set; }
        public string displayName { get; set; }
    }
}