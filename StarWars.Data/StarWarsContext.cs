using Microsoft.EntityFrameworkCore;
using StarWars.Domain;

namespace StarWars.Data
{
    public class StarWarsContext : DbContext
    {
        public StarWarsContext()
        { }

        public StarWarsContext(DbContextOptions options)
            : base(options)
        { }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<CharacterEpisode> CharacterEpisode { get; set; }
        public DbSet<CharacterFriend> CharacterFriend { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = StarWarsDb");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterEpisode>().HasKey(k => new { k.CharacterId, k.EpisodeId });
            modelBuilder.Entity<CharacterFriend>().HasKey(k => new { k.CharacterId, k.CharacterFriendId });

            modelBuilder.Entity<Episode>().HasData(
                new Episode() { Id = 1, EpisodeName = "NEWHOPE" },
                new Episode() { Id = 2, EpisodeName = "EMPIRE" },
                new Episode() { Id = 3, EpisodeName = "JEDI" }
            );

            modelBuilder.Entity<Character>().HasData(
                new Character() { Id = 1, Name = "Luke Skywalker" },
                new Character() { Id = 2, Name = "Darth Vader" },
                new Character() { Id = 3, Name = "Han Solo" },
                new Character() { Id = 4, Name = "Leia Organa" },
                new Character() { Id = 5, Name = "C-3PO" },
                new Character() { Id = 6, Name = "R2-D2" },
                new Character() { Id = 7, Name = "Wilhuff Tarkin" }
            );

            modelBuilder.Entity<CharacterEpisode>().HasData(
                new CharacterEpisode() { CharacterId = 1, EpisodeId = 1 },
                new CharacterEpisode() { CharacterId = 1, EpisodeId = 2 },
                new CharacterEpisode() { CharacterId = 1, EpisodeId = 3 },


                new CharacterEpisode() { CharacterId = 2, EpisodeId = 1 },
                new CharacterEpisode() { CharacterId = 2, EpisodeId = 2 },
                new CharacterEpisode() { CharacterId = 2, EpisodeId = 3 },

                new CharacterEpisode() { CharacterId = 3, EpisodeId = 1 },
                new CharacterEpisode() { CharacterId = 3, EpisodeId = 2 },
                new CharacterEpisode() { CharacterId = 3, EpisodeId = 3 },

            new CharacterEpisode() { CharacterId = 4, EpisodeId = 1 },
                new CharacterEpisode() { CharacterId = 4, EpisodeId = 2 },
            new CharacterEpisode() { CharacterId = 4, EpisodeId = 3 },

            new CharacterEpisode() { CharacterId = 7, EpisodeId = 1 },

                new CharacterEpisode() { CharacterId = 5, EpisodeId = 1 },
                new CharacterEpisode() { CharacterId = 5, EpisodeId = 2 },
                new CharacterEpisode() { CharacterId = 5, EpisodeId = 3 },

                new CharacterEpisode() { CharacterId = 6, EpisodeId = 1 },
            new CharacterEpisode() { CharacterId = 6, EpisodeId = 2 },
            new CharacterEpisode() { CharacterId = 6, EpisodeId = 3 }
            );


            modelBuilder.Entity<CharacterFriend>().HasData(
                new CharacterFriend() { CharacterId = 1, CharacterFriendId = 3 },
                new CharacterFriend() { CharacterId = 1, CharacterFriendId = 4 },
                new CharacterFriend() { CharacterId = 1, CharacterFriendId = 6 },
                new CharacterFriend() { CharacterId = 1, CharacterFriendId = 5 },


                new CharacterFriend() { CharacterId = 2, CharacterFriendId = 7 },

                new CharacterFriend() { CharacterId = 3, CharacterFriendId = 1 },
                new CharacterFriend() { CharacterId = 3, CharacterFriendId = 4 },
                new CharacterFriend() { CharacterId = 3, CharacterFriendId = 6 },

                new CharacterFriend() { CharacterId = 4, CharacterFriendId = 1 },
                new CharacterFriend() { CharacterId = 4, CharacterFriendId = 3 },
                new CharacterFriend() { CharacterId = 4, CharacterFriendId = 5 },
                new CharacterFriend() { CharacterId = 4, CharacterFriendId = 6 },

                new CharacterFriend() { CharacterId = 7, CharacterFriendId = 2 },

                new CharacterFriend() { CharacterId = 5, CharacterFriendId = 1 },
                new CharacterFriend() { CharacterId = 5, CharacterFriendId = 3 },
                new CharacterFriend() { CharacterId = 5, CharacterFriendId = 4 },
                new CharacterFriend() { CharacterId = 5, CharacterFriendId = 6 },

                new CharacterFriend() { CharacterId = 6, CharacterFriendId = 1 },
                new CharacterFriend() { CharacterId = 6, CharacterFriendId = 3 },
                new CharacterFriend() { CharacterId = 6, CharacterFriendId = 4 }
            );

        }

    }




}

