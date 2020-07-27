using Microsoft.EntityFrameworkCore;
using StarWars.Domain;
using System;

namespace StarWars.Data
{
    public class StarWarsContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }
        public DbSet<Episode> Episodes { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = StarWarsDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterEpisode>().HasKey(k => new { k.CharacterId, k.EpisodeId });
            modelBuilder.Entity<CharacterFriend>().HasKey(k => new { k.CharacterId, k.CharacterFriendId });
        }
    }
}
