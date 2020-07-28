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
        Task<Episode> GetEpisodeByNameAsync(string episodeName);

        void UpdateEpisode(Episode episode);
        void CreateEpisode(Episode episode);
        void DeleteEpisode(int episodeId);


        Task<bool> SaveChangesAsync();
        Episode GetEpisodeByName(string episodeName);
        Task<bool> EpisodeNameExistsAsync(string episodeName);
        void CreateEpisodeAndAssignCharacter(Character character, Episode episode);
        void CreateNewEpisodeForCharacter(Character character, Episode episode);
    }
}
