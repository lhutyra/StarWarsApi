using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StarWars.Api.Controllers;
using StarWars.Common.Model;
using StarWars.Domain;
using StarWars.Domain.Repositories;
using StarWars.Domain.Services;
using Xunit;

namespace StarWars.Tests.Functional
{
    public class CharacterControllerTests
    {
        private CharactersController _sut;
        private Mock<IEpisodeRepository> _episodeRepositoryMock = new Mock<IEpisodeRepository>();
        private Mock<ICharacterRepository> _characterRepositoryMock = new Mock<ICharacterRepository>();
        private Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private Mock<ICharacterService> _characterServiceMock = new Mock<ICharacterService>();
        public CharacterControllerTests()
        {
            _sut = new CharactersController(_characterRepositoryMock.Object, _episodeRepositoryMock.Object, _mapperMock.Object, _characterServiceMock.Object);
        }


        [Fact]
        public async void GetCharacter_NonExistingCharacter_NotFoundExpected()
        {
            int characterId = 1;
            _characterRepositoryMock.Setup(f => f.CharacterExistsAsync(characterId)).Returns(Task.FromResult(false));
            var result = await _sut.GetCharacter(characterId);
            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        public async void GetCharacter_ExistingCharacter_OkResultExpected()
        {
            int characterId = 1;
            _characterRepositoryMock.Setup(f => f.CharacterExistsAsync(characterId)).Returns(Task.FromResult(true));
            _characterServiceMock.Setup(f => f.GetCharactersMappedAsync(characterId))
                .Returns(Task.FromResult(new CharacterResult()));
            var result = await _sut.GetCharacter(characterId);
            Assert.IsType<OkObjectResult>(result);

        }

        [Fact]
        public async void GettingCharacterFriend_NonExistingBothCharacterAndFriend_NotFoundResultExpected()
        {
            int characterId = 1;
            int friendId = 2;
            _characterRepositoryMock.Setup(f => f.CharacterExistsAsync(characterId)).Returns(Task.FromResult(false));
            _characterRepositoryMock.Setup(f => f.CharacterExistsAsync(friendId)).Returns(Task.FromResult(false));
            var result = await _sut.DeleteFriendFromList(characterId, friendId);
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async void GettingCharacterFriend_NonExistingFriendExisting_NotFoundResultExpected()
        {
            int characterId = 1;
            int friendId = 2;
            _characterRepositoryMock.Setup(f => f.CharacterExistsAsync(characterId)).Returns(Task.FromResult(false));
            _characterRepositoryMock.Setup(f => f.CharacterExistsAsync(friendId)).Returns(Task.FromResult(true));
            var result = await _sut.DeleteFriendFromList(characterId, friendId);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GettingCharacterFriend_BothExisting_NotFoundResultExpected()
        {
            int characterId = 1;
            int friendId = 2;
            _characterRepositoryMock.Setup(f => f.CharacterExistsAsync(characterId)).Returns(Task.FromResult(false));
            _characterRepositoryMock.Setup(f => f.CharacterExistsAsync(friendId)).Returns(Task.FromResult(true));
            var result = await _sut.DeleteFriendFromList(characterId, friendId);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GettingCharacterFriend_BothExistingButHasFriendAssignedAlready_NotFoundResultExpected()
        {
            int characterId = 1;
            int friendId = 2;
            var character = new Character() { Id = characterId };
            var friend = new Character() { Id = friendId };
            _characterRepositoryMock.Setup(f => f.GetCharacterAsync(characterId)).Returns(Task.FromResult(character));
            _characterRepositoryMock.Setup(f => f.GetCharacterAsync(friendId)).Returns(Task.FromResult(friend));
            _characterRepositoryMock
                .Setup(f => f.HasFriendAlready(character, friend))
                .Returns(false);
            var result = await _sut.DeleteFriendFromList(characterId, friendId);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void GettingCharacterFriend_BothExistingAndHasntFriendAssignedAlready_NoContentResultExpected()
        {
            int characterId = 1;
            int friendId = 2;
            var character = new Character() {Id = characterId};
            var friend = new Character() { Id = friendId };
            _characterRepositoryMock.Setup(f => f.GetCharacterAsync(characterId)).Returns(Task.FromResult(character));
            _characterRepositoryMock.Setup(f => f.GetCharacterAsync(friendId)).Returns(Task.FromResult(friend));
            _characterRepositoryMock
                .Setup(f => f.HasFriendAlready(character, friend))
                .Returns(true);
            var result = await _sut.DeleteFriendFromList(characterId, friendId);
            _characterRepositoryMock.Verify(f => f.RemoveFriendFromCharacter(character, friend));
            Assert.IsType<NoContentResult>(result);
        }


        //[Microsoft.AspNetCore.Mvc.HttpPatch("{characterId}/friends/{friendId}")]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult<List<CharacterResult>>> AddFriendToCharacter(int characterId, int friendId)
        //{
        //    var character = await _characterRepository.GetCharacterAsync(characterId);
        //    if (character == null)
        //    {
        //        NotFound();
        //    }
        //    var friend = await _characterRepository.GetCharacterAsync(friendId);
        //    if (friend == null)
        //    {
        //        return NotFound();
        //    }

        //    try
        //    {
        //        _characterService.AddFriendToCharacter(character, friend);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }

        //    return CreatedAtAction(
        //        "GetCharacter",
        //        new { characterId },
        //        _mapper.Map<CharacterResult>(character));
        //}
    }
}