using Attendify.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendify.DataLayer.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : DbContext(options)
    {
        private readonly string _connectionString = configuration.GetConnectionString("AttendifyDB")
            ?? throw new InvalidOperationException("Connection string is missing");

        public required DbSet<Event> Events { get; set; }
        public required DbSet<RSVP> RSVPs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Event>().HasKey(e => e.Id);

            modelBuilder.Entity<RSVP>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<RSVP>()
                .HasOne(r => r.Event)
                .WithMany(e => e.RSVPs) // Ensure Event has a list of RSVPs
                .HasForeignKey(r => r.EventId) // Explicitly set EventId as the foreign key
                .OnDelete(DeleteBehavior.Cascade); // Define behavior on delete
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_connectionString);
            }
        }
    }
}
