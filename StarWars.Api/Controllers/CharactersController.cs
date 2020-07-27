using Microsoft.AspNetCore.Mvc;
using StarWars.Api.Model;
using StarWars.Data;
using StarWars.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarWars.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharactersController : ControllerBase
    {

        [HttpPost()]
        public async Task<ActionResult<Character>> CreateCharacter(CharacterCreation character)
        {
            using (StarWarsContext db = new StarWarsContext())
            {
                
                var newCharacter = new Character();
                newCharacter.CharacterEpisodes = new List<CharacterEpisode>() { new CharacterEpisode() { EpisodeId = 1, CharacterId = 1 } };
                newCharacter.Friends = new List<CharacterFriend>() { new CharacterFriend() { CharacterId = 1, CharacterFriendId = 2 } };
                newCharacter.Name = character.CharacterName;
                db.Characters.Add(newCharacter);
                await db.SaveChangesAsync();
                return CreatedAtRoute(
              "GetCharacter",
              new { characterId = newCharacter.Id },
              newCharacter);

            }
        }
    }
}