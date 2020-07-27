using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarWars.Api.Model
{
    public class CharacterResult
    {
        public string Name { get; set; }
        public List<string> Episodes { get; set; }
    }
}
