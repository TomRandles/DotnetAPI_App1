using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;

namespace SamuraiApp.Data.Database
{
    public class SamuraiContext : DbContext
    {
        // Need a constructor to take the Db context options and pass to base class
        // Will be able to instantiate with options defined in the Startup
        public SamuraiContext(DbContextOptions<SamuraiContext> options) : base (options)
        {

        }

        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<SamuraiBattleStat> SamuraiBattleStats { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Explicitly implement many-to-many relationship with extra payload
            builder.Entity<Samurai>()
                .HasMany(s => s.Battles)
                .WithMany(b => b.Samurais)
                .UsingEntity<BattleSamurai>             // Use this class to establish the many-to-many relationship
                (bs => bs.HasOne<Battle>().WithMany(),
                  bs => bs.HasOne<Samurai>().WithMany())  // EF core can do as far as this automatically
                .Property(bs => bs.DateJoined)
                .HasDefaultValueSql("getdate()");   // Reference tsql function

            // Horse table has no DbSet. Breaks table naming convention. Use this command to fix.
            // Migration will rename
            builder.Entity<Horse>().ToTable("Horses");

            // Keyless entity to map to a Db view. ToView - stops EF Core from creating a table
            builder.Entity<SamuraiBattleStat>().HasNoKey().ToView("SamuraiBattleStats");
        }
    }
}