using System;
using System.Collections.Generic;
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
        public DbSet<Team> Teams { get; set; }
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


            modelBuilder.Entity<Team>().HasKey(t => t.Id);
            modelBuilder.Entity<Team>().Property(t => t.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Comment>().HasKey(c => c.Id);
            modelBuilder.Entity<Comment>().Property(c => c.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Comment>().HasOne<User>(u => u.User);
            
            modelBuilder.Entity<Sport>().HasKey(s => s.Id);
            modelBuilder.Entity<Sport>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Sport>().HasMany<Team>(s => s.Teams).WithOne(t=>t.Sport).OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Match>().HasKey(m => m.Id);
            modelBuilder.Entity<Match>().Property(m => m.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Match>().HasOne<Team>(m => m.Local);
            modelBuilder.Entity<Match>().HasOne<Team>(m => m.Visitor);

            modelBuilder.Entity<Session>().HasKey(l => l.Token);
            modelBuilder.Entity<Session>().Property(l => l.Token).ValueGeneratedOnAdd();

            modelBuilder.Entity<Favorite>().HasKey(f => f.Id);
            modelBuilder.Entity<Favorite>().Property(f => f.Id).ValueGeneratedOnAdd();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          // optionsBuilder.UseSqlServer(@"Server=(localdb)\SQLEXPRESS;Database=SPORTS;User Id =Rafael;Password=Rafael;");
           //optionsBuilder.UseLazyLoadingProxies(true);
        }
    }
}
