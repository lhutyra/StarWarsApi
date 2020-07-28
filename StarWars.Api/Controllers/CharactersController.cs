﻿using System;
using Microsoft.AspNetCore.Mvc;
using StarWars.Api.Model;
using StarWars.Data;
using StarWars.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using StarWars.Common.Model;
using StarWars.Domain.Repositories;

namespace StarWars.Api.Controllers
{
    [ApiController]
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


        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IEnumerable<CharacterResult>> Get()
        {
            return await _characterService.GetCharactersMappedAsync();
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
            _characterService.AddFriendToCharacter(character, friend);
            return CreatedAtAction(
                "GetCharacter",
                new { characterId },
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
