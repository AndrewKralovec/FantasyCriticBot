using System;
using System.Collections.Generic;
using System.Text;

namespace FantasyBot.Models
{
    public class GameRelease
    {
        public GameJson Game { get; set; }
        public string PlayerName { get; set; }
        public string PublisherName { get; set; }
        public DateTime? ReleaseDate { get => Game.releaseDate; }
        public string FormatedDate { get => ReleaseDate?.ToString("dddd, dd MMMM yyyy"); }
        public string GameName { get => Game.gameName; }
        public GameRelease(GameJson game) => Game = game;
        public GameRelease(PublisherJson publisher)
        {
            PublisherName = publisher.publisherName;
            PlayerName = publisher.playerName;
            Game = publisher.NextGameReleased();
        }
    }
}
