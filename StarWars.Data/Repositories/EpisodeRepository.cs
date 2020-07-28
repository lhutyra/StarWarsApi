using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarWars.Domain;
using StarWars.Domain.Repositories;

namespace StarWars.Data.Repositories
{
    public class EpisodeRepository : IEpisodeRepository, IDisposable
    {
        private StarWarsContext _context;

        public EpisodeRepository(StarWarsContext context)
        {
            _context = context;
        }

        public async Task<bool> EpisodeExistsAsync(int episodeId)
        {
            return await _context.Episodes.AnyAsync(a => a.Id == episodeId);
        }

        public async Task<bool> EpisodeNameExistsAsync(string episodeName)
        {
            return await _context.Episodes.AnyAsync(a => a.EpisodeName == episodeName);
        }

        public async Task<IEnumerable<Episode>> GetEpisodeAsync()
        {
            return await _context.Episodes.ToListAsync();
        }

        public async Task<Episode> GetEpisodeAsync(int episodeId)
        {

            return await _context.Episodes
                .FirstOrDefaultAsync(a => a.Id == episodeId);
        }

        public async Task<Episode> GetEpisodeByNameAsync(string episodeName)
        {
            var result = await _context.Episodes.FirstOrDefaultAsync(f => f.EpisodeName == episodeName);
            return result;
        }
        public Episode GetEpisodeByName(string episodeName)
        {
            var result = _context.Episodes.FirstOrDefault(f => f.EpisodeName == episodeName);
            return result;
        }

        public void UpdateEpisode(Episode episode)
        {
            _context.Episodes.Update(episode);
        }

        public void CreateEpisode(Episode episode)
        {
            _context.Episodes.Add(episode);
        }
        public void CreateNewEpisodeForCharacter(Character character, Episode episode)
        {
            _context.Episodes.Add(episode);
        }

        public void CreateEpisodeAndAssignCharacter(Character character, Episode episode)
        {
            
            _context.CharacterEpisode.Add(new CharacterEpisode() {Character = character, Episode = episode});
        }

        public void DeleteEpisode(int episodeId)
        {
            var episodeToRemove = _context.Episodes.FirstOrDefault(f => f.Id == episodeId);
            if (episodeToRemove == null)
            {
                throw new InvalidOperationException("Element not found");
            }
            var connectionToRemove = _context.CharacterEpisode.Where(f => f.EpisodeId == episodeId);
            _context.RemoveRange(connectionToRemove);
            _context.Episodes.Remove(episodeToRemove);
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
