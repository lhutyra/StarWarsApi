using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Moq;
using StarWars.Domain;
using StarWars.Domain.Repositories;
using StarWars.Domain.Services;
using Xunit;

namespace StarWars.Tests
{
    public class CharacterServiceTests
    {
        private readonly ICharacterService _sut;
        private readonly Mock<ICharacterRepository> _characterRepositoryMock;
        private readonly Mock<IEpisodeRepository> _episodeRepositoryMock;
        public CharacterServiceTests()
        {
            _characterRepositoryMock = new Mock<ICharacterRepository>();
            _episodeRepositoryMock = new Mock<IEpisodeRepository>();
            _sut = new CharacterService(_characterRepositoryMock.Object, _episodeRepositoryMock.Object);
        }

        [Fact]
        public void AddingFriendToCharacter_ShouldThrowException()
        {
            var anyCharacter = It.IsAny<Character>();
            _characterRepositoryMock.Setup(f => f.HasFriendAlready(anyCharacter, anyCharacter)).Returns(true);
            Assert.Throws<InvalidOperationException>(() => _sut.AddFriendToCharacter(new Character(), new Character()));
        }

    }
}
