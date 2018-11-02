using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Sports.Domain;

namespace Sports.Repository.Context
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options) {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Competitor> Competitors { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Session> Logins { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().HasMany<Comment>().WithOne(c => c.User).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasMany<Favorite>().WithOne(f => f.User).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasMany<Session>().WithOne(s => s.User).OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Competitor>().HasKey(t => t.Id);
            modelBuilder.Entity<Competitor>().Property(t => t.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Competitor>().HasMany<CompetitorScore>().WithOne(cs=>cs.Competitor).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Competitor>().HasMany<Favorite>().WithOne(f => f.Competitor).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>().HasKey(c => c.Id);
            modelBuilder.Entity<Comment>().Property(c => c.Id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Sport>().HasKey(s => s.Id);
            modelBuilder.Entity<Sport>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Sport>().HasMany<Competitor>(s => s.Competitors).WithOne(t=>t.Sport).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Match>().HasKey(m => m.Id);
            modelBuilder.Entity<Match>().Property(m => m.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Match>().HasMany<Comment>(m => m.Comments).WithOne(c => c.Match).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Match>().HasMany<CompetitorScore>(m => m.Competitors).WithOne().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Session>().HasKey(l => l.Token);
            modelBuilder.Entity<Session>().Property(l => l.Token).ValueGeneratedOnAdd();

            modelBuilder.Entity<Favorite>().HasKey(f => f.Id);
            modelBuilder.Entity<Favorite>().Property(f => f.Id).ValueGeneratedOnAdd();
        }
        
    }
}
