using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarWars.Api.Model;
using StarWars.Data;
using StarWars.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using StarWars.Domain.Repositories;


namespace StarWars.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EpisodesController : ControllerBase
    {
        private readonly IEpisodeRepository _episodeRepository;
        private readonly IMapper _mapper;

        public EpisodesController(IEpisodeRepository episodeRepository, IMapper mapper)
        {
            _episodeRepository = episodeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<Episode>> Get()
        {
            return await _episodeRepository.GetEpisodeAsync();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int episodeId)
        {
            _episodeRepository.DeleteEpisode(episodeId);
            await _episodeRepository.SaveChangesAsync();
            return NoContent();

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
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var entityEpisode = _mapper.Map<Episode>(episode);
                if (await _episodeRepository.EpisodeNameExistsAsync(episode.EpisodeName))
                {
                    return BadRequest("Episode name exists already");
                }
                _episodeRepository.CreateEpisode(entityEpisode);
                await _episodeRepository.SaveChangesAsync();
                return CreatedAtAction(
                    "GetEpisode",
                    new { episodeId = entityEpisode.Id },
                    entityEpisode);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

