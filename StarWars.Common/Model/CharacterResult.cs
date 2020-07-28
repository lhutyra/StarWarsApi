using System.Collections.Generic;

namespace StarWars.Common.Model
{
    public class CharacterResult
    {
        public string Name { get; set; }
        public List<string> Episodes { get; set; }
        public List<string> Friends { get; set; }
    }
}
