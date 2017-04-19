using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace mmmsl.Models
{
    public class MmmslDatabase : DbContext
    {
        public MmmslDatabase(DbContextOptions<MmmslDatabase> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Division> Divisions { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Penalty> Penalties { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RosterPlayer> RosterPlayers { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Penalty>()
                .HasIndex(penalty => penalty.MisconductCode)
                .IsUnique();

            modelBuilder.Entity<Profile>()
                .HasIndex(profile => profile.Email)
                .IsUnique();

            modelBuilder.Entity<Role>().HasKey(role => new {
                role.ProfileId,
                role.Name
            });

            modelBuilder.Entity<Role>()
                .HasOne<Profile>()
                .WithMany(profile => profile.Roles);

            modelBuilder.Entity<RosterPlayer>().HasKey(rp => new {
                rp.ProfileId,
                rp.TeamId
            });

            modelBuilder.Entity<RosterPlayer>()
                .HasOne(rp => rp.Profile)
                .WithMany()
                .HasForeignKey(rp => rp.ProfileId);

            modelBuilder.Entity<RosterPlayer>()
                .HasOne(rp => rp.Team)
                .WithMany(team => team.Roster)
                .HasForeignKey(rp => rp.TeamId);

            var foreignKeys = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys());

            foreach (var relationship in foreignKeys) {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}