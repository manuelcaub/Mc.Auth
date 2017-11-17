using System;
using Mc.Auth.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mc.Auth.Database.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasKey(user => user.Id);

            builder.Entity<User>()
                .Property(user => user.UserName)
                .IsRequired();

            builder.Entity<User>()
                .Property(user => user.Password)
                .IsRequired();
        }
    }
}
