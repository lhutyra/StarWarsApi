using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.Domain.Repositories
{
    public interface ICharacterRepository
    {
        Task<bool> CharacterExistsAsync(int characterId);
        Task<IEnumerable<Character>> GetCharacterAsync();
        Task<Character> GetCharacterAsync(int characterId);
        void CreateCharacter(Character character);
        void UpdateCharacter(Character character);
        Task<bool> SaveChangesAsync();
    }
}
