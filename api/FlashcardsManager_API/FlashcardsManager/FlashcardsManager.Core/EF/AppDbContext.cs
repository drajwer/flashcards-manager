using System;
using FlashcardsManager.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FlashcardsManager.Core.EF
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole, string> //IdentityDbContext<User, IdentityRole<string>, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public AppDbContext(string connectionString) : 
            base(new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connectionString).Options)
        {
        }
        public AppDbContext()
        {
        }
        public DbSet<Flashcard> Flashcards { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserProgress> UserProgress { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserProgress>().HasKey(up => new { up.UserId, up.FlashcardId });
        }
    }

    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext Create()
        {
            return new AppDbContext("DefaultConnection");
        }

        public AppDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
