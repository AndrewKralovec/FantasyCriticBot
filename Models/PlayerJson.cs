using Newtonsoft.Json;
using System;

namespace FantasyBot.Models
{
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
}