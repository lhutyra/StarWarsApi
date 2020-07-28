using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StarWars.Api.Controllers;
using StarWars.Common.Model;
using StarWars.Domain;
using StarWars.Domain.Repositories;
using Xunit;

namespace StarWars.Tests.Functional
{
    public class EpisodeControllerTests
    {
        private EpisodesController _sut;
        private Mock<IEpisodeRepository> _episodeRepositoryMock = new Mock<IEpisodeRepository>();
        private Mock<IMapper> _mapperMock = new Mock<IMapper>();
        public EpisodeControllerTests()
        {
            _sut = new EpisodesController(_episodeRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void RemovingEpisode_NonExistingEpisode_NotFoundReturn()
        {
            _episodeRepositoryMock.Setup(f => f.EpisodeExistsAsync(1)).Returns(Task.FromResult(false));
            var result = await _sut.Delete(1);
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async void RemovingEpisode_ExistingEpisode_NoContentReturn()
        {
            _episodeRepositoryMock.Setup(f => f.EpisodeExistsAsync(1)).Returns(Task.FromResult(true));
            var result = await _sut.Delete(1);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void UpdategEpisode_EpisodeWithNameExists_BadRequestReturn()
        {
            var episodeName = "testEpisode";
            _episodeRepositoryMock.Setup(f => f.EpisodeNameExistsAsync(episodeName)).Returns(Task.FromResult(true));

            var result = await _sut.Update(1, new EpisodeDto() { EpisodeName = episodeName });
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void UpdateEpisode_EpisodeWithNonExistName_CreateAtActionResult()
        {
            var episodeName = "testEpisode";
            _episodeRepositoryMock.Setup(f => f.EpisodeNameExistsAsync(episodeName)).Returns(Task.FromResult(false));
            _mapperMock.Setup(f => f.Map<Episode>(It.IsAny<EpisodeDto>())).Returns(new Episode());
            var result = await _sut.Update(1, new EpisodeDto() { EpisodeName = episodeName });
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async void GetEpisode_NonExistingEpisode_NotFoundReturn()
        {
            var episodeId = 1;
            _episodeRepositoryMock.Setup(f => f.EpisodeExistsAsync(episodeId)).Returns(Task.FromResult(false));
            var result = await _sut.GetEpisode(episodeId);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetEpisode_ExistingEpisode_OkReturn()
        {
            var episodeId = 1;
            _episodeRepositoryMock.Setup(f => f.EpisodeExistsAsync(episodeId)).Returns(Task.FromResult(true));
            var result = await _sut.GetEpisode(episodeId);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetEpisode_ExistingEpisode_RepositoryCalled()
        {
            var episodeId = 1;
            _episodeRepositoryMock.Setup(f => f.EpisodeExistsAsync(episodeId)).Returns(Task.FromResult(true));
            var result = _sut.GetEpisode(episodeId);
            _episodeRepositoryMock.Verify(f => f.GetEpisodeAsync(episodeId));
        }
    }
}
