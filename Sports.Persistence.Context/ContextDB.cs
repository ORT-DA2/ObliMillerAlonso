using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Sports.Domain;

namespace Sports.Persistence.Context
{
    public class ContextDB : DbContext
    {
        public ContextDB(DbContextOptions options) : base(options) {
        }
        public DbSet<User> Users { get; set; }
        //public object ContextOption { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasAlternateKey(u => u.UserName);
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFGetStarted.ConsoleApp.NewDb;Trusted_Connection=True;");
            }
            optionsBuilder.UseLazyLoadingProxies(true);

            //this.Database.EnsureCreated();
        }
    }
}
