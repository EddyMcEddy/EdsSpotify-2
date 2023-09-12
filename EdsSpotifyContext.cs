using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EdsSpotify
{
    public class EdsSpotifyContext : DbContext
    {
        public DbSet<Albums> Albums { get; set; }
        public DbSet<Bands> Bands { get; set; }
        public DbSet<Songs> Songs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("server=localhost;database=EdsSpotify");



            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            optionsBuilder.UseLoggerFactory(loggerFactory);

        }
    }
}


