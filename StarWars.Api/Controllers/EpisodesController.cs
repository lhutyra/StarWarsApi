using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarWars.Api.Model;
using StarWars.Data;
using StarWars.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars.Domain.Repositories;


namespace StarWars.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EpisodesController : ControllerBase
    {
        private readonly IEpisodeRepository _episodeRepository;

        public EpisodesController(IEpisodeRepository episodeRepository)
        {
            _episodeRepository = episodeRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Episode>> Get()
        {
            return await _episodeRepository.GetEpisodeAsync();
        }

        [HttpGet("{episodeId}")]
        public async Task<ActionResult<Episode>> GetEpisode(

            int episodeId)
        {
            return await _episodeRepository.GetEpisodeAsync(episodeId);
        }

        [HttpPost()]
        public async Task<ActionResult<Episode>> CreateEpisode([FromBody] EpisodeCreation episode)
        {
            using (StarWarsContext db = new StarWarsContext())
            {
                Episode newEntity = new Episode();
                newEntity.EpisodeName = episode.Title;
                db.Episodes.Add(newEntity);
                await db.SaveChangesAsync();
                return CreatedAtRoute(
                    "GetEpisode",
                    new {episodeId = newEntity.Id},
                    episode);
            }
        }
    }
}

