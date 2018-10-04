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
        public DbSet<Team> Teams { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Session> Logins { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var cascadeFKs = modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys()).Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;


            
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().HasMany<Comment>().WithOne(c => c.User).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasMany<Favorite>().WithOne(f => f.User).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasMany<Session>().WithOne(s => s.User).OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Team>().HasKey(t => t.Id);
            modelBuilder.Entity<Team>().Property(t => t.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Team>().HasMany<Match>(t => t.LocalMatches).WithOne(m => m.Local).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Team>().HasMany<Match>(t => t.VisitorMatches).WithOne(m => m.Visitor).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Team>().HasMany<Favorite>().WithOne(f => f.Team).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>().HasKey(c => c.Id);
            modelBuilder.Entity<Comment>().Property(c => c.Id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Sport>().HasKey(s => s.Id);
            modelBuilder.Entity<Sport>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Sport>().HasMany<Team>(s => s.Teams).WithOne().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Match>().HasKey(m => m.Id);
            modelBuilder.Entity<Match>().Property(m => m.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Match>().HasMany<Comment>(m => m.Comments).WithOne().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Session>().HasKey(l => l.Token);
            modelBuilder.Entity<Session>().Property(l => l.Token).ValueGeneratedOnAdd();

            modelBuilder.Entity<Favorite>().HasKey(f => f.Id);
            modelBuilder.Entity<Favorite>().Property(f => f.Id).ValueGeneratedOnAdd();
        }
        
    }
}
