using EventFinderServer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventFinderServer.DAL
{
    public class EventFinderDBC : IdentityDbContext<User>
    {
        public EventFinderDBC(DbContextOptions<EventFinderDBC> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserFavorites> UserFavorites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                        .Property(e => e.Languages)
                        .HasConversion<int>();

            modelBuilder.Entity<UserFavorites>()
            .HasKey(uf => new { uf.UserId, uf.EventId});
                modelBuilder.Entity<UserFavorites>()
                    .HasOne(uf => uf.User)
                    .WithMany(u => u.Favorites)
                    .HasForeignKey(uf => uf.UserId);
                modelBuilder.Entity<UserFavorites>()
                    .HasOne(uf => uf.Event)
                    .WithMany(e => e.UserFavorites)
                    .HasForeignKey(uf => uf.EventId);

            modelBuilder.Entity<Event>()
            .HasMany(e => e.Messages)
            .WithOne(m => m.Event);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Event>().ToTable("Event");
            modelBuilder.Entity<Message>().ToTable("Message");
        }
    }
}
