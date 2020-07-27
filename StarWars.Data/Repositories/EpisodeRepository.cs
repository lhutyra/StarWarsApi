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
            return await _context.Episodes.FirstOrDefaultAsync(f => f.EpisodeName == episodeName);
        }

        public void UpdateEpisode(Episode episode)
        {

        }

        public void CreateEpisode(Episode episode)
        {
            _context.Episodes.Add(episode);
        }

        public void DeleteEpisode(int episodeId)
        {
            var episodeToRemove = _context.Episodes.Where(f => f.Id == episodeId).FirstOrDefault();
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
