using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Domain
{
    public class CharacterFriend
    {
        public int CharacterId { get; set; }
        public int CharacterFriendId { get; set; }
        public Character Character { get; set; }
    }
}
