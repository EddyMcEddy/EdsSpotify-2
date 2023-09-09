using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EdsSpotify
{
    public class EdsSpotifyContext : DbContext
    {
        public DbSet<Albums> Albums { get; set; }
        // Define a method required by EF that will configure our connection
        // to the database.
        //
        // DbContextOptionsBuilder is provided to us. We then tell that object
        // we want to connect to a postgres database named suncoast_movies on
        // our local machine.
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


