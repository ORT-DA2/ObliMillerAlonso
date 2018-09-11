using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Sports.Domain;
using Microsoft.EntityFrameworkCore.Proxies;

namespace Sports.Persistence.Context
{
    public class ContextDB : DbContext
    {
        public ContextDB() : base() { }
        public DbSet<User> Users { get; set; }
        public object ContextOption { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasOne(u => u.Password);
            modelBuilder.Entity<User>().HasOne(u => u.UserName);
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(true);
        }
    }
}
