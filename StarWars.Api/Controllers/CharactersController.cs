using Microsoft.AspNetCore.Mvc;
using StarWars.Api.Model;
using StarWars.Data;
using StarWars.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using StarWars.Domain.Repositories;

namespace StarWars.Api.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
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


        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IEnumerable<CharacterResult>> Get()
        {
            var dbResult = await _characterRepository.GetCharacterAsync();
            var mappedResult = _mapper.Map<List<CharacterResult>>(dbResult);
            return mappedResult;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("{characterId}")]
        public async Task<ActionResult<CharacterResult>> GetCharacter(

            int characterId)
        {

            var result = await _characterRepository.GetCharacterAsync(characterId);
            var mappedResult = _mapper.Map<CharacterResult>(result);
            return Ok(mappedResult);
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("{characterId}/friends")]
        public async Task<ActionResult<List<CharacterResult>>> GetFriendsOfCharacter(int characterId)
        {
            var character = await _characterRepository.GetCharacterAsync(characterId);
            var mappedResult = _mapper.Map<List<CharacterResult>>(character.Friends);
            return Ok(mappedResult);
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("{characterId}/episodes")]
        public async Task<ActionResult<List<Episode>>> GetEpisodesWithCharacter(int characterId)
        {
            var character = await _characterRepository.GetCharacterAsync(characterId);
            var mappedResult = _mapper.Map<List<EpisodeResult>>(character.CharacterEpisodes.Select(f => f.Episode));
            return Ok(mappedResult);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("{characterId}/episodes")]
        public async Task<ActionResult<ActionResult<Character>>> AssignNewEpisodesWithCharacter(int characterId, EpisodeCreation episodeForCharacter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var character = _characterRepository.GetCharacter(characterId);
            if (character == null)
            {
                return NotFound();
            }
            _episodeRepository.CreateEpisodeAndAssignCharacter(character, _mapper.Map<Episode>(episodeForCharacter));
            await _episodeRepository.SaveChangesAsync();
            return CreatedAtAction(
                "GetCharacter",
                new { characterId = characterId },
                character);
        }
        [Microsoft.AspNetCore.Mvc.HttpPost("{characterId}/{episodeId}")]
        public async Task<ActionResult<ActionResult<Character>>> AssignExistedEpisodeWithCharacter(int characterId, int episodeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var character = _characterRepository.GetCharacter(characterId);
            if (character == null)
            {
                return NotFound();
            }

            var episode = await _episodeRepository.GetEpisodeAsync(episodeId);
            if (episode == null)
            {
                return NotFound();
            }
            _episodeRepository.CreateEpisodeAndAssignCharacter(character, episode);
            await _episodeRepository.SaveChangesAsync();
            return CreatedAtAction(
                "GetCharacter",
                new { characterId = characterId },
                character);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost()]
        public async Task<ActionResult<Character>> CreateCharacter(CharacterCreation character)
        {
            using (StarWarsContext db = new StarWarsContext())
            {

                var newCharacter = new Character
                {
                    CharacterEpisodes =
                        character.Episodes.Select(f => new CharacterEpisode() { EpisodeId = _episodeRepository.GetEpisodeByName(f).Id }).ToList(),
                    Friends = new List<CharacterFriend>()
                    {
                        new CharacterFriend() {CharacterId = 1, CharacterFriendId = 2}
                    },
                    Name = character.CharacterName
                };
                db.Characters.Add(newCharacter);
                await db.SaveChangesAsync();
                return CreatedAtAction(
              "GetCharacter",
              new { characterId = newCharacter.Id },
              newCharacter);
            }
        }
    }
}