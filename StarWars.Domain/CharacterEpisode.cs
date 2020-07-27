using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Domain
{
    public class CharacterEpisode
    {
        public int EpisodeId { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }
        public Episode Episode { get; set; }
    }
}
