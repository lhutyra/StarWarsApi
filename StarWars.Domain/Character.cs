using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Domain
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CharacterEpisode> CharacterEpisodes { get; set; }
        public List<CharacterFriend> Friends { get; set; }
    }
}
