using Microsoft.AspNetCore.Mvc;
using StarWars.Api.Model;
using StarWars.Data;
using StarWars.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using StarWars.Domain.Repositories;

namespace StarWars.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly IEpisodeRepository _episodeRepository;
        private readonly IMapper _mapper;
        public CharactersController(ICharacterRepository repository, IEpisodeRepository episodeRepository, IMapper mapper)
        {
            _characterRepository = repository;
            _episodeRepository = episodeRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IEnumerable<Episode>> Get()
        {
            return await _episodeRepository.GetEpisodeAsync();
        }

        [HttpGet("{characterId}")]
        public async Task<ActionResult<CharacterResult>> GetEpisode(

            int characterId)
        {

            var result = await _characterRepository.GetCharacterAsync(characterId);
            var mappedResult = _mapper.Map<CharacterResult>(result);
            return Ok(mappedResult);
        }

        [HttpPost()]
        public async Task<ActionResult<Character>> CreateCharacter(CharacterCreation character)
        {
            using (StarWarsContext db = new StarWarsContext())
            {

                var newCharacter = new Character
                {
                    CharacterEpisodes =
                        character.Episodes.Select(f => new CharacterEpisode() { EpisodeId = _episodeRepository.GetEpisodeByNameAsync(f).Id }).ToList(),
                    Friends = new List<CharacterFriend>()
                    {
                        new CharacterFriend() {CharacterId = 1, CharacterFriendId = 2}
                    },
                    Name = character.CharacterName
                };
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