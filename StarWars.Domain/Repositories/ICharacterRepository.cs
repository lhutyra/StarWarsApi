using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.Domain.Repositories
{
    public interface ICharacterRepository
    {
        Task<bool> CharacterExistsAsync(int characterId);
        Task<bool> CharacterNameExistsAsync(string characterName);
        Task<IEnumerable<Character>> GetCharacterAsync();
        Task<Character> GetCharacterAsync(int characterId);
        Character GetCharacter(int characterId);
        void CreateCharacter(Character character);
        void UpdateCharacter(Character character);
        void AddFriendToCharacter(Character character,Character friend);
        bool HasFriendAlready(Character character, Character friend);
        Task<bool> SaveChangesAsync();
        IQueryable<Character> GetFriendsOfCharacter(int characterId);
    }
}
