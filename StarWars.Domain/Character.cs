using System;
using System.Collections.Generic;
using System.Text;

namespace StarWars.Domain
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Episode> Episodes { get; set; }
        public List<Character> Friends { get; set; }
    }
}
