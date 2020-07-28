using System;
using System.Collections.Generic;
using System.Linq;
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
            if (characterId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(characterId));
            }
            return await _context.Characters.AnyAsync(a => a.Id == characterId);
        }

        public async Task<bool> HasEpisodeAssignedAsync(int characterId, int episodeId)
        {
            return await _context.CharacterEpisode.AnyAsync(f => f.CharacterId == characterId && f.EpisodeId == episodeId);
        }

        public async Task<bool> CharacterNameExistsAsync(string characterName)
        {
            if (characterName is null)
            {
                throw new ArgumentNullException(nameof(characterName));
            }
            return await _context.Characters.AnyAsync(a => a.Name == characterName);
        }

        public async Task<IEnumerable<Character>> GetCharacterAsync()
        {
            return await _context.Characters.Include(c => c.CharacterEpisodes).ThenInclude(z => z.Episode).Include(z => z.Friends).ThenInclude(z => z.Character).ToListAsync();
        }

        public async Task<Character> GetCharacterAsync(int characterId)
        {
            if (characterId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(characterId));
            }
            return await _context.Characters.Include(f => f.CharacterEpisodes).ThenInclude(t => t.Episode)
                .FirstOrDefaultAsync(a => a.Id == characterId);
        }

        public Character GetCharacter(int characterId)
        {
            if (characterId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(characterId));
            }
            return _context.Characters.Include(f => f.CharacterEpisodes).ThenInclude(t => t.Episode)
                .FirstOrDefault(a => a.Id == characterId);
        }

        public void UpdateCharacter(Character character)
        {
            _context.Characters.Update(character);
        }

        public void AddFriendToCharacter(Character character, Character friend)
        {
            _context.Add(new CharacterFriend() { CharacterId = character.Id, CharacterFriendId = friend.Id });
        }
        public void RemoveFriendFromCharacter(Character character, Character friend)
        {
            _context.CharacterFriend.RemoveRange(_context.CharacterFriend.Where(f => f.CharacterFriendId == friend.Id && f.CharacterId == character.Id));
        }

        public bool HasFriendAlready(Character character, Character friend)
        {
            return _context.CharacterFriend.Any(f => f.CharacterId == character.Id && f.CharacterFriendId == friend.Id);
        }
        public IQueryable<Character> GetFriendsOfCharacter(int characterId)
        {
            var characterFriends = _context.CharacterFriend.Where(f => f.CharacterId == characterId).Select(f => f.CharacterFriendId);
            return _context.Characters.Where(c => characterFriends.Contains(c.Id));

        }

        public void CreateCharacter(Character character)
        {
            _context.Characters.Add(character);
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

