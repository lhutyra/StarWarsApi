using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars.Common.Model;

namespace StarWars.Domain.Services
{
    public interface ICharacterService
    {
        Task AddFriendToCharacterAsync(Character character, Character friend);
        Task AssignNewEpisodeToCharacter(Character character, Episode episode);
        Task<IEnumerable<CharacterResult>> GetCharactersMappedAsync();
        Task<CharacterResult> GetCharactersMappedAsync(int characterId);
        IEnumerable<Character> GetFriendsOfCharacter(int characterId);
    }
}