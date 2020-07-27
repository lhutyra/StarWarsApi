using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.Domain.Repositories
{
    public interface IEpisodeRepository : IDisposable
    {
        Task<bool> EpisodeExistsAsync(int episodeId);

        Task<IEnumerable<Episode>> GetEpisodeAsync();

        Task<Episode> GetEpisodeAsync(int episodeId);

        void UpdateEpisode(Episode episode);
        void CreateEpisode(Episode episode);

        Task<bool> SaveChangesAsync();
    }
}
