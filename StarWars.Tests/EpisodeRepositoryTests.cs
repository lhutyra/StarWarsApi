using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StarWars.Data;
using StarWars.Data.Repositories;
using StarWars.Domain;
using Xunit;

namespace StarWars.Tests
{
    public class EpisodeRepositoryTests
    {
        [Fact]
        public async void CanInsertEpisode()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("InsertNewEpisode");

            using (var context = new StarWarsContext(builder.Options))
            {
                var repository = new EpisodeRepository(context);
                repository.CreateEpisode(new Episode() { EpisodeName = "test episode" });
                await repository.SaveChangesAsync();

            };
            using (var context2 = new StarWarsContext(builder.Options))
            {
                Assert.Equal(1, context2.Episodes.Count());
            }

        }
    }
}
