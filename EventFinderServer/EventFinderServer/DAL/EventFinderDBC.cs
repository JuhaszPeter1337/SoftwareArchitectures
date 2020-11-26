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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                        .Property(e => e.Languages)
                        .HasConversion<int>();

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Event>().ToTable("Event");
            modelBuilder.Entity<Message>().ToTable("Message");
        }
    }
}
