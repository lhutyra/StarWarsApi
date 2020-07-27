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

        /// <summary>
        /// Assign friend id to selected characterId
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns>route to action</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost("{characterId}/friends/{friendId}")]

        public async Task<ActionResult<List<CharacterResult>>> AddFriendToCharacter(int characterId)
        {
            var character = await _characterRepository.GetCharacterAsync(characterId);
            if (character == null)
            {
                NotFound();
            }
            var friend = await _characterRepository.GetCharacterAsync(characterId);
            if (friend == null)
            {
                return NotFound();
            }
            _characterRepository.AddFriendToCharacter(character, friend);
            await _characterRepository.SaveChangesAsync();
            return CreatedAtAction(
                "GetCharacter",
                new { characterId = characterId },
                _mapper.Map<CharacterResult>(character));
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("{characterId}/episodes")]
        public async Task<ActionResult<List<Episode>>> GetEpisodesWithCharacter(int characterId)
        {
            var character = await _characterRepository.GetCharacterAsync(characterId);
            var mappedResult = _mapper.Map<List<EpisodeResult>>(character.CharacterEpisodes.Select(f => f.Episode));
            return Ok(mappedResult);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("{characterId}/episodes")]
        public async Task<ActionResult<ActionResult<CharacterResult>>> AssignNewEpisodesWithCharacter(int characterId, EpisodeCreation episodeForCharacter)
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

            if (await _characterRepository.CharacterNameExistsAsync(character.Name))
            {
                return BadRequest();
            }

            var entityToCreate = _mapper.Map<Character>(character);
            _characterRepository.CreateCharacter(entityToCreate);
            await _characterRepository.SaveChangesAsync();
            return CreatedAtAction(
          "GetCharacter",
          new { characterId = entityToCreate.Id },
           _mapper.Map<CharacterResult>(entityToCreate));
        }
    }
}
