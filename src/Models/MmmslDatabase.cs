using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
using System;

namespace mmmsl.Models
{
    public class MmmslDatabase : DbContext
    {
        public MmmslDatabase(DbContextOptions<MmmslDatabase> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<BoardMember> BoardMembers { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Penalty> Penalties { get; set; }
        public DbSet<PenaltyDefinition> PenaltyDefinitions { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RosterPlayer> RosterPlayers { get; set; }
        public DbSet<TeamManager> TeamManagers { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BoardMember>()
                .HasOne(boardMember => boardMember.Profile)
                .WithMany();

            modelBuilder.Entity<Penalty>()
                .HasOne(penalty => penalty.PenaltyCard)
                .WithMany()
                .HasForeignKey(penalty => penalty.MisconductCode);

            modelBuilder.Entity<PenaltyDefinition>()
                .HasKey(penaltyDefinition => penaltyDefinition.MisconductCode);

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

            modelBuilder.Entity<TeamManager>().HasKey(manager => new {
                manager.ProfileId,
                manager.TeamId
            });

            modelBuilder.Entity<RosterPlayer>()
                .HasOne(player => player.Profile)
                .WithMany()
                .HasForeignKey(player => player.ProfileId);

            modelBuilder.Entity<RosterPlayer>()
                .HasOne(player => player.Team)
                .WithMany(team => team.Roster)
                .HasForeignKey(player => player.TeamId);

            modelBuilder.Entity<RosterPlayer>().HasKey(player => new {
                player.ProfileId,
                player.TeamId
            });

            modelBuilder.Entity<TeamManager>()
                .HasOne(manager => manager.Profile)
                .WithMany()
                .HasForeignKey(manager => manager.ProfileId);

            modelBuilder.Entity<TeamManager>()
                .HasOne(manager => manager.Team)
                .WithMany(team => team.Managers)
                .HasForeignKey(manager => manager.TeamId);

            var foreignKeys = modelBuilder.Model
                .GetEntityTypes()
                .Where(e => !CascadeDeleteEntityWhiteList(e.ClrType))
                .SelectMany(e => e.GetForeignKeys());

            foreach (var relationship in foreignKeys) {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private bool CascadeDeleteEntityWhiteList(Type type)
        {
            return type == typeof(RosterPlayer)
                || type == typeof(TeamManager);
        }
    }
}