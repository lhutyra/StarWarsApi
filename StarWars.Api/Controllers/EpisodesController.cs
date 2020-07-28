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
using Microsoft.AspNetCore.Http;
using StarWars.Common.Model;
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

        /// <summary>
        /// List of all episodes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Episode>> Get()
        {
            return await _episodeRepository.GetEpisodeAsync();
        }

        /// <summary>
        /// Delete episode with selected id
        /// </summary>
        /// <param name="episodeId">Episode id to delete</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{episodeId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int episodeId)
        {
            if (!await _episodeRepository.EpisodeExistsAsync(episodeId))
            {
                return NotFound();
            }
            _episodeRepository.DeleteEpisode(episodeId);
            await _episodeRepository.SaveChangesAsync();
            return NoContent();

        }

        /// <summary>
        /// Update episode
        /// </summary>
        /// <param name="episodeId">Episode id to update</param>
        /// <param name="episodeDto">new episode data</param>
        /// <returns></returns>
        [HttpPatch("{episodeId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Update(int episodeId, [FromBody] EpisodeDto episodeDto)
        {
            if (await _episodeRepository.EpisodeNameExistsAsync(episodeDto.EpisodeName))
            {
                return BadRequest($"Episode with {episodeDto.EpisodeName} name exists already");
            }
            var episode = _mapper.Map<Episode>(episodeDto);
            episode.Id = episodeId;
            _episodeRepository.UpdateEpisode(episode);
            await _episodeRepository.SaveChangesAsync();
            return CreatedAtAction(
                "GetEpisode",
                new { episodeId = episodeId },
                episode);

        }

        /// <summary>
        /// Get episode by Id
        /// </summary>
        /// <param name="episodeId">selected episode id</param>
        /// <returns>Episode</returns>

        [HttpGet("{episodeId}")]
        public async Task<IActionResult> GetEpisode(

            int episodeId)
        {
            if (!await _episodeRepository.EpisodeExistsAsync(episodeId))
            {
                return NotFound();
            }
            return Ok(await _episodeRepository.GetEpisodeAsync(episodeId));
        }


        /// <summary>
        /// Create new episode
        /// </summary>
        /// <param name="episode">Episode name</param>
        /// <returns>ActionResult</returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost()]
        public async Task<ActionResult<Episode>> CreateEpisode([FromBody] EpisodeDto episode)
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
                    return BadRequest($"Episode name: {episode.EpisodeName} already exists");
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

