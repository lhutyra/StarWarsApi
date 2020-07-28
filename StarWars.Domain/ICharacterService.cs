using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars.Common.Model;

namespace StarWars.Domain
{
    public interface ICharacterService
    {
        void AddFriendToCharacter(Character character, Character friend);
        void AssignNewEpisodeToCharacter(Character character, Episode episode);
        Task<IEnumerable<CharacterResult>> GetCharactersMappedAsync();
    }
}