using System.Collections.Generic;

namespace StarWars.Common.Model
{
    public class CharacterResult
    {
        public CharacterResult()
        {
            Episodes = new List<string>();
            Friends = new List<string>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Episodes { get; set; }
        public List<string> Friends { get; set; }
    }
}
