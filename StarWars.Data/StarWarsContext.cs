using Microsoft.EntityFrameworkCore;
using StarWars.Domain;
using System;

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
            //modelBuilder.Entity<CharacterWithFriend>().HasKey(k => new { k.CharacterId, k.CharacterFriendId });
            //modelBuilder.Entity<CharacterWithFriend>().HasOne(c => c.Character).WithOne().OnDelete(DeleteBehavior.ClientNoAction);
            //modelBuilder.Entity<CharacterWithFriend>().HasOne(c => c.CharacterFriend).WithOne().OnDelete(DeleteBehavior.ClientNoAction);
            


        }
    }
}
