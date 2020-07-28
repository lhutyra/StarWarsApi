using System;
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
        public async void AddingExistingFriendToCharacter_ShouldThrowException()
        {
            var anyCharacter = new Character() {Name = "test"};
            _characterRepositoryMock.Setup(f => f.HasFriendAlready(anyCharacter, anyCharacter)).Returns(true);
            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.AddFriendToCharacterAsync(anyCharacter, anyCharacter));
        }

        [Fact]
        public async void AddingSelfToCharacter_ShouldThrowException()
        {
            var me = new Character() { Id = 1 };
            var self = new Character() { Id = 1 };
            _characterRepositoryMock.Setup(f => f.HasFriendAlready(me, self)).Returns(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.AddFriendToCharacterAsync(me, self));
        }

    }
}
