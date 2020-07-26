using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarWars.Api.Model;
using StarWars.Data;
using StarWars.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace StarWars.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EpisodesController : ControllerBase
    {

        [HttpGet]
        public IEnumerable<Episode> Get()
        {
            using (StarWarsContext db = new StarWarsContext())
            {
                return db.Episodes;
            }
        }
        [HttpGet("{episodeId}")]
        public async Task<ActionResult<Episode>> GetEpisode(

          int episodeId)
        {
            using (StarWarsContext db = new StarWarsContext())
            {
                var result = await db.Episodes.FirstOrDefaultAsync(f => f.Id == episodeId);
                return Ok(result);
            }
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
               new { episode = newEntity.Id },
             episode);
            }
        }
    }
}
