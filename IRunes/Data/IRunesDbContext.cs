using IRunes.Models;
using Microsoft.EntityFrameworkCore;

namespace IRunes.Data
{
    public class IRunesDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Track> Tracks { get; set; }

        public DbSet<AlbumTrack> AlbumsTracks { get; set; }

        public DbSet<UserAlbum> UsersAlbums { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=IRunes;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlbumTrack>().HasKey(x => new { x.AlbumId, x.TrackId });  
            
            modelBuilder.Entity<UserAlbum>().HasKey(x => new { x.UserId, x.AlbumId });
        }


    }
}
