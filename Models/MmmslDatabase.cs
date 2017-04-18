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
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Division> Divisions { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
            
            var foreignKeys = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys());

            foreach (var relationship in foreignKeys) {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}