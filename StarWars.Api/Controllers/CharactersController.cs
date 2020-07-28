using System;
using Microsoft.AspNetCore.Mvc;
using StarWars.Api.Model;
using StarWars.Data;
using StarWars.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using StarWars.Common.Model;
using StarWars.Domain.Repositories;

namespace StarWars.Api.Controllers
{
    [ApiController]
    [Produces("application/json", "application/xml")]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly IEpisodeRepository _episodeRepository;
        private readonly ICharacterService _characterService;
        private readonly IMapper _mapper;
        public CharactersController(ICharacterRepository repository, IEpisodeRepository episodeRepository, IMapper mapper, ICharacterService characterService)
        {
            _characterRepository = repository;
            _episodeRepository = episodeRepository;
            _mapper = mapper;
            _characterService = characterService;
        }

        /// <summary>
        /// List all characters
        /// </summary>
        /// <returns>List of all characters</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IEnumerable<CharacterResult>> Get()
        {
            return await _characterService.GetCharactersMappedAsync();
        }

        /// <summary>
        /// Get character by Id
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet("{characterId}")]
        public async Task<ActionResult<CharacterResult>> GetCharacter(

            int characterId)
        {

            var result = await _characterService.GetCharactersMappedAsync(characterId);
            return Ok(result);
        }

        /// <summary>
        /// List friends of character
        /// </summary>
        /// <param name="characterId">characterId</param>
        /// <returns>List of friends</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Microsoft.AspNetCore.Mvc.HttpGet("{characterId}/friends")]
        public async Task<ActionResult<List<CharacterResult>>> GetFriendsOfCharacter(int characterId)
        {
            var character = await _characterRepository.GetCharacterAsync(characterId);
            var mappedResult = _mapper.Map<List<CharacterResult>>(character.Friends);
            return Ok(mappedResult);
        }

        /// <summary>
        /// Remove character from character friend list
        /// </summary>
        /// <param name="characterId">Selected character id</param>
        /// <param name="friendId">Character Id to remove</param>
        /// <returns>ActionResult</returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Microsoft.AspNetCore.Mvc.HttpDelete("{characterId}/friends/{friendId}")]

        public async Task<IActionResult> DeleteFriendFromList(int characterId, int friendId)
        {
            var character = await _characterRepository.GetCharacterAsync(characterId);
            var characterFriend = await _characterRepository.GetCharacterAsync(friendId);
            if (character == null)
            {
                return NotFound();
            }
            if (characterFriend == null)
            {
                return NotFound();
            }

            if (!_characterRepository.HasFriendAlready(character, characterFriend))
            {
                throw new InvalidOperationException($"{character.Name} doesn't has friend {characterFriend.Name}");
            }
            _characterRepository.RemoveFriendFromCharacter(character, characterFriend);
            try
            {
                await _characterRepository.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


        }

        /// <summary>
        /// Assign friend id to selected characterId
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns>route to action</returns>
        [Microsoft.AspNetCore.Mvc.HttpPatch("{characterId}/friends/{friendId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<CharacterResult>>> AddFriendToCharacter(int characterId, int friendId)
        {
            var character = await _characterRepository.GetCharacterAsync(characterId);
            if (character == null)
            {
                NotFound();
            }
            var friend = await _characterRepository.GetCharacterAsync(friendId);
            if (friend == null)
            {
                return NotFound();
            }

            try
            {
                _characterService.AddFriendToCharacter(character, friend);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return CreatedAtAction(
                "GetCharacter",
                new { characterId },
                _mapper.Map<CharacterResult>(character));
        }

        /// <summary>
        /// List episodes for selected character
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Microsoft.AspNetCore.Mvc.HttpGet("{characterId}/episodes")]
        public async Task<ActionResult<List<Episode>>> GetEpisodesWithCharacter(int characterId)
        {
            var character = await _characterRepository.GetCharacterAsync(characterId);
            if (character == null)
            {
                return NotFound();
            }
            var mappedResult = _mapper.Map<List<EpisodeResult>>(character.CharacterEpisodes.Select(f => f.Episode));
            return Ok(mappedResult);
        }

        /// <summary>
        /// Create new episode and assign it to selected character
        /// </summary>
        /// <param name="characterId">Selected character</param>
        /// <param name="episodeForCharacter">Episode name to create</param>
        /// <returns>Action result</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost("{characterId}/episodes")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ActionResult<CharacterResult>>> AssignNewEpisodesWithCharacter(int characterId, EpisodeDto episodeForCharacter)
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
            _characterService.AssignNewEpisodeToCharacter(character, _mapper.Map<Episode>(episodeForCharacter));
            await _episodeRepository.SaveChangesAsync();
            return CreatedAtAction(
                "GetCharacter",
                new { characterId = characterId },
                character);
        }
        /// <summary>
        /// Assign existing episode to character
        /// </summary>
        /// <param name="characterId">Selected character</param>
        /// <param name="episodeId">Episode Id to assign to character</param>
        /// <returns>ActionResult</returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Microsoft.AspNetCore.Mvc.HttpPatch("{characterId}/{episodeId}")]
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

            if (await _characterRepository.HasEpisodeAssignedAsync(characterId, episodeId))
            {
                return BadRequest("Episode already assigned to character");
            }
            _episodeRepository.CreateEpisodeAndAssignCharacter(character, episode);
            await _episodeRepository.SaveChangesAsync();
            return CreatedAtAction(
                "GetCharacter",
                new {  characterId },
                character);
        }

        /// <summary>
        /// Create new character
        /// </summary>
        /// <param name="character">Character data(name)</param>
        /// <returns>ActionResult</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost()]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
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


        /// <summary>
        /// Update character name
        /// </summary>
        /// <param name="characterId">Character id to update</param>
        /// <param name="characterDto">Character name</param>
        /// <returns></returns>
        [HttpPatch("{characterId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Update(int characterId, [FromBody] CharacterCreation characterDto)
        {
            if (await _characterRepository.CharacterNameExistsAsync(characterDto.Name))
            {
                return BadRequest($"Character with {characterDto.Name} name exists already");
            }
            var character = _mapper.Map<Character>(characterDto);
            character.Id = characterId;
            _characterRepository.UpdateCharacter(character);
            await _episodeRepository.SaveChangesAsync();
            return CreatedAtAction(
                "GetCharacter",
                new { characterId = character.Id },
                character);

        }
    }
}
