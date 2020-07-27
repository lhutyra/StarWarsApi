using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarWars.Domain;
using StarWars.Domain.Repositories;

namespace StarWars.Data.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private StarWarsContext _context;

        public CharacterRepository(StarWarsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CharacterExistsAsync(int characterId)
        {
            return await _context.Characters.AnyAsync(a => a.Id == characterId);
        }

        public async Task<IEnumerable<Character>> GetCharacterAsync()
        {
            return await _context.Characters.ToListAsync();
        }

        public async Task<Character> GetCharacterAsync(int characterId)
        {

            return await _context.Characters
                .FirstOrDefaultAsync(a => a.Id == characterId);
        }

        public void UpdateCharacter(Character Character)
        {

        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
    }
}

