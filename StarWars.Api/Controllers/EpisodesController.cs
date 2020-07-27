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
                var entityEpisode = _mapper.Map<Episode>(episode);
                _episodeRepository.CreateEpisode(entityEpisode);
                await _episodeRepository.SaveChangesAsync();
                return CreatedAtRoute(
                    "GetEpisode",
                    new { episodeId = 1 },
                    entityEpisode);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

