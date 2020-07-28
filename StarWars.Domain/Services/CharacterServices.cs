using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarWars.Common.Model;
using StarWars.Domain.Repositories;

namespace StarWars.Domain.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly IEpisodeRepository _episodeRepository;

        public CharacterService(ICharacterRepository characterRepository, IEpisodeRepository episodeRepository)
        {
            _characterRepository = characterRepository;
            _episodeRepository = episodeRepository;
        }
        public async Task AddFriendToCharacterAsync(Character character, Character friend)
        {
            if (_characterRepository.HasFriendAlready(character, friend))
            {
                throw new InvalidOperationException($"{friend?.Name} is already friend of {character.Name}");
            }

            if (character.Id == friend.Id)
            {
                throw new InvalidOperationException($"Cannot add friend of myself");
            }

            _characterRepository.AddFriendToCharacter(character, friend);
            await _characterRepository.SaveChangesAsync();
        }

        public async void AssignNewEpisodeToCharacter(Character character, Episode episode)
        {
            if (await _episodeRepository.EpisodeNameExistsAsync(episode.EpisodeName))
            {
                throw new InvalidOperationException($"{episode.EpisodeName} already exists");
            }
            _episodeRepository.CreateEpisodeAndAssignCharacter(character, episode);
        }

        public IEnumerable<Character> GetFriendsOfCharacter(int characterId)
        {
            return _characterRepository.GetFriendsOfCharacter(characterId).ToList();
        }

        public async Task<IEnumerable<CharacterResult>> GetCharactersMappedAsync()
        {
            var characterList = await _characterRepository.GetCharacterAsync();
            List<CharacterResult> characterResultList = new List<CharacterResult>();

            foreach (var character in characterList)
            {
                CharacterResult characterResult = new CharacterResult();
                var friendOfCharacter = _characterRepository.GetFriendsOfCharacter(character.Id).Select(f => f.Name).ToList();
                characterResult.Id = character.Id;
                characterResult.Name = character.Name;
                characterResult.Friends = friendOfCharacter;
                characterResult.Episodes = character.CharacterEpisodes.Select(f => f.Episode.EpisodeName).ToList();
                characterResultList.Add(characterResult);

            }

            return characterResultList;
        }
        public async Task<CharacterResult> GetCharactersMappedAsync(int characterId)
        {
            var character = await _characterRepository.GetCharacterAsync(characterId);
            CharacterResult characterResult = new CharacterResult();


            var friendOfCharacter = _characterRepository.GetFriendsOfCharacter(character.Id).Select(f => f.Name).ToList();
            characterResult.Id = character.Id;
            characterResult.Name = character.Name;
            characterResult.Friends = friendOfCharacter;
            characterResult.Episodes = character.CharacterEpisodes.Select(f => f.Episode.EpisodeName).ToList();

            return characterResult;
        }

    }
}
